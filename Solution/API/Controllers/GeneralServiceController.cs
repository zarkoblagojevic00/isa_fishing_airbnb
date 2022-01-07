using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Entities.Abstractions;
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

            var reservationHistory = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId)
                .Select(x => new ReservationHistoryDTO()
                {
                    Reservation = x,
                    Report = x.ReportId == null ? null : UoW.GetRepository<IReportReadRepository>().GetAll().First(y => y.ReportId == x.ReportId),
                    Mark = x.MarkId == null ? null : UoW.GetRepository<IMarkReadRepository>().GetAll().First(y => y.MarkId == x.MarkId)
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
                return BadRequest(Responses.NotFound);
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
    }
}
