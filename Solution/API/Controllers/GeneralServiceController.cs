﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using NHibernate;
using Services;
using Services.Constants;
using Services.HtmlWriter;
using Services.Validators;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GeneralServiceController : AdvancedController
    {
        public GeneralServiceController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetReservationHistory(int serviceId)
        {
            if (!CheckOwnerShip(serviceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var service = UoW.GetRepository<IServiceReadRepository>()
                .GetById(serviceId);
            var reservationHistory = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId)
                .Select(x => new ReservationHistoryDTO()
                {
                    Reservation = x,
                    Report = x.ReportId == null ? null : UoW.GetRepository<IReportReadRepository>().GetAll().First(y => y.ReportId == x.ReportId),
                    Mark = x.MarkId == null ? null : UoW.GetRepository<IMarkReadRepository>().GetAll().First(y => y.MarkId == x.MarkId),
                    Role = service.ServiceType == ServiceType.Boat 
                        ? GetRole(x.ReservationId, false)
                        : string.Empty
                });

            return Ok(reservationHistory);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetUserInfoFromReservation(int reservationId)
        {
            var reservation = UoW.GetRepository<IReservationReadRepository>().GetById(reservationId);

            if (reservation == null)
            {
                return BadRequest(Responses.NotFound);
            }

            if (!CheckOwnerShip(reservation.ServiceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var user = UoW.GetRepository<IUserReadRepository>().GetById(reservation.UserId);
            var city = UoW.GetRepository<ICityReadRepository>().GetById(user.CityId);

            var result = new UserInfoDTO()
            {
                Address = user.Address,
                City = city.Name,
                Country = UoW.GetRepository<ICountryReadRepository>().GetById(city.CountryId).Name,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Name = user.Name,
                Surname = user.Surname
            };

            return Ok(result);
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult SubmitReport(ReportDTO newReport)
        {
            var reservation = UoW.GetRepository<IReservationReadRepository>().GetById(newReport.ReservationId);

            if (reservation == null)
            {
                return BadRequest(Responses.NotFound);
            }

            if (!CheckOwnerShip(reservation.ServiceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            if (reservation.EndDateTime > DateTime.Now && !reservation.IsCanceled)
            {
                return BadRequest(Responses.OngoingReservation);
            }

            if (reservation.ReportId != null)
            {
                return BadRequest(Responses.ReportAlreadySubmitted);
            }

            var report = CreateNewReport(newReport);

            try
            {
                UoW.BeginTransaction();

                UoW.GetRepository<IReportWriteRepository>().Add(report);
                reservation.ReportId = report.ReportId;

                UoW.GetRepository<IReservationWriteRepository>().Update(reservation);

                if (!report.ShownUp)
                {
                    new PenalizationService(UoW).PenalizeUser(reservation.ReservationId);
                }

                if (report.ShownUp && report.SuggestPenalty)
                {
                    var issue = new Issue()
                    {
                        CreatedDateTime = DateTime.Now,
                        IsReviewed = false,
                        IssuedEntityId = reservation.UserId,
                        UserId = GetUserIdFromCookie(),
                        Reason = report.ReportText
                    };
                    UoW.GetRepository<IIssueWriteRepository>().Add(issue);
                }

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok(Responses.Ok);
        }
        
        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult CreateReservationForUser(NewReservationDTO reservationDto)
        {
            var user = UoW.GetRepository<IUserReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Email == reservationDto.UserMail && x.UserType == UserType.Registered && x.IsAccountVerified && x.IsAccountActive);
            if (user == null)
            {
                return BadRequest("Email: " + Responses.NotFound);
            }

            if (!CheckOwnerShip(reservationDto.ServiceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            UoW.BeginTransaction();

            var service = new Service();
            try
            {
                service = new ServiceLocker(UoW).ObtainLockedService(reservationDto.ServiceId);
            }
            catch
            {
                return BadRequest(Responses.UnavailableRightNow);
            }
            
            if (service.AvailableTo != null && service.AvailableFrom != null)
            {
                if (!(service.AvailableFrom <= reservationDto.StartDateTime &&
                      service.AvailableTo >= reservationDto.EndDateTime))
                {
                    return BadRequest(Responses.ServiceNotAvailableAtGivenTime);
                }
            }

            if (service.ServiceType == ServiceType.Boat && reservationDto.IsCaptain == null)
            {
                return BadRequest(Responses.MissingResponsibility);
            }

            if(service.ServiceType == ServiceType.Adventure)
            {
                var unavailabilityService = new UserUnavailabilityValidationService(UoW);
                bool isAvailable = unavailabilityService.IsInstructorAvailable(service.OwnerId, reservationDto.StartDateTime, reservationDto.EndDateTime);
                if(!isAvailable)
                    return BadRequest(Responses.DatesOverlap);
            }

            var union = GetRelevantDateIntervals(service.ServiceId, user.UserId);
            var intervalToCheck = new CalendarItem()
            {
                StartDateTime = reservationDto.StartDateTime,
                EndDateTime = reservationDto.EndDateTime
            };

            var overlaps = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(intervalToCheck, union);
            if (overlaps)
            {
                return BadRequest(Responses.DatesOverlap);
            }

            try
            {
                var newReservation = CreateNewReservation(reservationDto, user.UserId);
                
                UoW.GetRepository<IReservationWriteRepository>().Add(newReservation);

                if (service.ServiceType == ServiceType.Boat)
                {
                    var boatResDetails = new BoatReservationDetail()
                    {
                        BoatOwnerResponsibilityType = reservationDto.IsCaptain.Value
                            ? BoatOwnerResponsibilityType.Captain
                            : BoatOwnerResponsibilityType.FirstAssistant,
                        RelevantId = newReservation.ReservationId,
                        IsPromo = false,
                    };
                    UoW.GetRepository<IBoatReservationDetailWriteRepository>().Add(boatResDetails);
                }

                UoW.Commit();

                NotifyUser(newReservation, service, user);
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }
            return Ok(Responses.Ok);
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult MarkServiceUnavailable(ServiceUnavailableDTO marker)
        {
            if (!CheckOwnerShip(marker.ServiceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            UoW.BeginTransaction();

            var service = new ServiceLocker(UoW).ObtainLockedService(marker.ServiceId);

            var relevantDates = GetServiceRelevantDates(marker.ServiceId);
            var calendarItemToCheck = new CalendarItem()
            {
                StartDateTime = marker.StartDateTime,
                EndDateTime = marker.EndDateTime
            };
            var response = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(calendarItemToCheck, relevantDates);
            if (response)
            {
                return BadRequest(Responses.DatesOverlap);
            }

            var newReservation = new Reservation()
            {
                IsServiceUnavailableMarker = true,
                ServiceId = marker.ServiceId,
                UserId = GetUserIdFromCookie(),
                StartDateTime = marker.StartDateTime,
                EndDateTime = marker.EndDateTime
            };
            
            UoW.GetRepository<IReservationWriteRepository>().Add(newReservation);
            UoW.Commit();

            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetCurrentReservations(int serviceId)
        {
            if (!CheckOwnerShip(serviceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var ongoingReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.StartDateTime < DateTime.Now && x.EndDateTime > DateTime.Now)
                .OrderBy(x => x.StartDateTime);

            return Ok(ongoingReservations);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetNonPromoReservations(int serviceId)
        {
            if (!CheckOwnerShip(serviceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var ongoingReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && !x.IsPromo)
                .OrderBy(x => x.StartDateTime);

            return Ok(ongoingReservations);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetBusinessSummary(int serviceId)
        {
            if (!CheckOwnerShip(serviceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime < DateTime.Now && !x.IsCanceled && !x.IsServiceUnavailableMarker);
            var marks = UoW.GetRepository<IMarkReadRepository>()
                .GetAll()
                .Where(x => x.MarkId.In(reservations
                    .Where(y => y.MarkId != null)
                    .Select(y => y.MarkId.Value)
                    .Distinct()
                    .ToArray()));

            var sysInfoBackPeriod = UoW.GetRepository<ISystemConfigReadRepository>()
                .GetValue<int>("BusinessInfoBackPeriod");

            var summary = new BusinessReportService().GetBusinessSummary(reservations, marks, sysInfoBackPeriod);

            return Ok(summary);
        }

        [HttpGet]
        public IActionResult GetServicesByType(ServiceType serviceType)
        {

            var services = UoW.GetRepository<IServiceReadRepository>().GetAll()
                .Where(x => x.ServiceType == serviceType);

            List<ServiceInfoDTO> serviceInfos = new();

            foreach (var service in services)
            {
                var owner = UoW.GetRepository<IUserReadRepository>().GetAll()
                    .Where(x => x.UserId == service.OwnerId).FirstOrDefault();
                var dto = new ServiceInfoDTO()
                {
                    ServiceId = service.ServiceId,
                    Name = service.Name,
                    OwnerId = service.OwnerId,
                    OwnerName = owner.Name,
                    OwnerSurname = owner.Surname,
                    Address = service.Address,
                    PromoDescription = service.PromoDescription,
                    PricePerDay = service.PricePerDay,
                    Capacity = service.Capacity,
                    TermsOfUse = service.TermsOfUse,
                    AdditionalEquipment = service.AdditionalEquipment,
                    IsPercentageTakenFromCanceledReservations = service.IsPercentageTakenFromCanceledReservations,
                    PercentageToTake = service.PercentageToTake,
                    ServiceType = service.ServiceType
                };
                serviceInfos.Add(dto);
            }

            return Ok(serviceInfos);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetRegisteredMails()
        {
            var registeredUsers = UoW.GetRepository<IUserReadRepository>()
                .GetAll()
                .Where(x => x.IsAccountActive && x.IsAccountVerified && x.UserType == UserType.Registered)
                .Select(x => x.Email);

            return Ok(registeredUsers);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetReservationsWithBasicUserInfo()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll();

            var users = UoW.GetRepository<IUserReadRepository>().GetAll();

            var userReservations = reservations
                .Join(adventures, r => r.ServiceId, a => a.ServiceId, (r, a) => new { r, a })
                .Join(users, ra => ra.r.UserId, u => u.UserId, (ra, u) => new { ra, u })
                .Select(reservation => new ReservationUserDTO
                {
                    AdditionalEquipment = reservation.ra.r.AdditionalEquipment,
                    IsCanceled = reservation.ra.r.IsCanceled,
                    IsPromo = reservation.ra.r.IsPromo,
                    IsServiceUnavailableMarker = reservation.ra.r.IsServiceUnavailableMarker,
                    MarkId = reservation.ra.r.MarkId,
                    Price = reservation.ra.r.Price,
                    ReportId = reservation.ra.r.ReportId,
                    ReservationId = reservation.ra.r.ReservationId,
                    ReservedDateTime = reservation.ra.r.ReservedDateTime,
                    ServiceId = reservation.ra.r.ServiceId,
                    ServiceName = reservation.ra.a.Name,
                    UserId = reservation.ra.r.UserId,
                    UsersName = reservation.u.Name,
                    UsersSurname = reservation.u.Surname,
                    UsersPhoneNumber = reservation.u.PhoneNumber,
                    ServiceFrom = reservation.ra.r.StartDateTime,
                    ServiceTo = reservation.ra.r.EndDateTime,
                    Capacity = reservation.ra.a.Capacity,
                    UsersEmail = reservation.u.Email,
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            return Ok(distinctReservations);
        }

        private Report CreateNewReport(ReportDTO report)
        {
            return new Report()
            {
                CreatedDateTime = DateTime.Now,
                ReportText = report.ReportText,
                ShownUp = report.ShownUp,
                SuggestPenalty = report.SuggestPenalty
            };
        }

        private Reservation CreateNewReservation(NewReservationDTO reservation, int userId)
        {
            return new Reservation()
            {
                UserId = userId,
                ServiceId = reservation.ServiceId,
                ReservedDateTime = DateTime.Now,
                AdditionalEquipment = reservation.AdditionalEquipment,
                Price = reservation.Price,
                StartDateTime = reservation.StartDateTime,
                EndDateTime = reservation.EndDateTime
            };
        }

        private void NotifyUser(Reservation newReservation, Service service, User user)
        {
            var html = HtmlWriter.ReservationNotificationTemplate(newReservation, service);

            var mailingService = new MailingService(UoW)
            {
                Body = html,
                Receiver = user.Email,
                Title = "Info"
            };

            mailingService.Send();
        }

        private IEnumerable<CalendarItem> GetRelevantDateIntervals(int serviceId, int userId)
        {
            var userReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.UserId == userId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var serviceReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var serviceQuickActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            return userReservations.Union(serviceReservations).Union(serviceQuickActions);
        }

        private IEnumerable<CalendarItem> GetServiceRelevantDates(int serviceId)
        {
            var relevantReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => !x.IsCanceled && x.ServiceId == serviceId && x.EndDateTime >= DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var relevantActions = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime >= DateTime.Now);

            return relevantReservations.Union(relevantActions);
        }

        private string GetRole(int relevantId, bool isPromo)
        {
            var relevantBoatRes = UoW.GetRepository<IBoatReservationDetailReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.IsPromo == isPromo && x.RelevantId == relevantId);
            if (relevantBoatRes == null)
                return string.Empty;

            return relevantBoatRes.BoatOwnerResponsibilityType == BoatOwnerResponsibilityType.Captain
                ? "Captain"
                : "First Assistant";
        }
    }
}
