using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using API.Mappers;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Constants;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InstructorController : AdvancedController
    {
        public InstructorController(IUnitOfWork uow) : base(uow)
        { }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true, UserType.Instructor })]
        public IActionResult AddAvailabilityPeriod(AvailabilityPeriodDTO period)
        {
            int ownerId = GetUserIdFromCookie();

            var additionalInstructorInfos = UoW.GetRepository<IAdditionalInstructorInfoWriteRepository>();
            var currentAdditionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>()
                .GetAll()
                .Where(x => x.UserId == ownerId).FirstOrDefault();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
                .GetAll()
                .Where(x => x.OwnerId == ownerId);

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId.In(adventures.Select(y => y.OwnerId).ToArray())  && !x.IsCanceled);

            var promoActionDates = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId.In(adventures.Select(y => y.OwnerId).ToArray()) && x.IsTaken);

            if (reservationDates.Any() || promoActionDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            currentAdditionalInstructorInfo.AvailableFrom = period.StartDateTime;
            currentAdditionalInstructorInfo.AvailableTo = period.EndDateTime;

            try
            {
                UoW.BeginTransaction();
                additionalInstructorInfos.Update(currentAdditionalInstructorInfo);
                UoW.Commit();
            }
            catch(Exception e)
            {
                UoW.Rollback();
                return BadRequest("Error");
            }

            return Ok(ownerId);
        }


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetMarkedReservationsWithBasicUserInfo()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll();

            var marks = UoW.GetRepository<IMarkReadRepository>()
                .GetAll();

            var users = UoW.GetRepository<IUserReadRepository>().GetAll();

            var userReservations = reservations
                .Join(adventures, r => r.ServiceId, a => a.ServiceId, (r, a) => new { r, a })
                .Join(marks, ra => ra.r.MarkId, m => m.MarkId, (ra, m) => new {ra, m })
                .Join(users, ram => ram.ra.r.UserId, u => u.UserId, (ram, u) => new { ram, u })
                .Select(reservation => new ReservationUserDTO
                {
                    AdditionalEquipment = reservation.ram.ra.r.AdditionalEquipment,
                    IsCanceled = reservation.ram.ra.r.IsCanceled,
                    IsPromo = reservation.ram.ra.r.IsPromo,
                    IsServiceUnavailableMarker = reservation.ram.ra.r.IsServiceUnavailableMarker,
                    MarkId = reservation.ram.ra.r.MarkId,
                    Mark = reservation.ram.m.GivenMark,
                    Price = reservation.ram.ra.r.Price,
                    ReportId = reservation.ram.ra.r.ReportId,
                    ReservationId = reservation.ram.ra.r.ReservationId,
                    ReservedDateTime = reservation.ram.ra.r.ReservedDateTime,
                    ServiceId = reservation.ram.ra.r.ServiceId,
                    ServiceName = reservation.ram.ra.a.Name,
                    UserId = reservation.ram.ra.r.UserId,
                    UsersName = reservation.u.Name,
                    UsersSurname = reservation.u.Surname,
                    UsersPhoneNumber = reservation.u.PhoneNumber,
                    ServiceFrom = reservation.ram.ra.r.StartDateTime,
                    ServiceTo = reservation.ram.ra.r.EndDateTime,
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            return Ok(distinctReservations);
        }


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
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

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetFinishedReservationsWithBasicUserInfo()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(res => res.EndDateTime < DateTime.Now);

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
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            return Ok(distinctReservations);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetReservationForUser(int reservationId)
        {
            var reservation = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(res => res.ReservationId == reservationId)
                .FirstOrDefault();

            var service = UoW.GetRepository<IServiceReadRepository>().GetAll()
                .Where(ser => ser.ServiceId == reservation.ServiceId)
                .FirstOrDefault();

            var user = UoW.GetRepository<IUserReadRepository>().GetAll()
                .Where(u => u.UserId == reservation.UserId)
                .FirstOrDefault();

            var reservationForUser = new ReservationForUserDTO
            {
                ServiceId = reservation.ServiceId,
                ServiceName = service.Name,
                UserEmail = user.Email,
            };

            return Ok(reservationForUser);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult UpdateAdditionalInstructorInfo(AdditionalInstructorInfoDTO availability)
        {
            var userId = GetUserIdFromCookie();

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetAll()
                .Where(x => x.UserId == userId).FirstOrDefault();

            additionalInstructorInfo.AvailableFrom = availability.Start;
            additionalInstructorInfo.AvailableTo = availability.End;

            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IAdditionalInstructorInfoWriteRepository>().Update(additionalInstructorInfo);
                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok(Responses.Ok);
        }


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetAdditionalInstructorInfo()
        {
            var userId = GetUserIdFromCookie();

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetAll()
                .Where(x => x.UserId == userId).FirstOrDefault();


            AvailabilityPeriodDTO availability = new AvailabilityPeriodDTO
            {
                StartDateTime = additionalInstructorInfo.AvailableFrom == null ? DateTime.Now.AddDays(1) : additionalInstructorInfo.AvailableFrom.Value,
                EndDateTime = additionalInstructorInfo.AvailableTo == null ? DateTime.Now.AddDays(2) : additionalInstructorInfo.AvailableTo.Value,
            };

            return Ok(availability);
        }


        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
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

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok(Responses.Ok);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetAvailabilityPeriods()
        {
            var userId = GetUserIdFromCookie();

            var instructorAvailabilityPeriods = UoW.GetRepository<IUserAvailabilityReadRepository>().GetAll()
                .Where(x => x.UserId == userId)
                .Where(x => x.PeriodEnd > DateTime.Now);

            

            return Ok(instructorAvailabilityPeriods);
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult AddInstructorAvailabilityPeriod(UserAvailabilityPeriodDTO availabilityPeriod)
        {
            var userId = GetUserIdFromCookie();

            availabilityPeriod.UserId = userId;
            availabilityPeriod.PeriodStart = availabilityPeriod.PeriodStart.ToLocalTime();
            availabilityPeriod.PeriodEnd = availabilityPeriod.PeriodEnd.ToLocalTime();

            var unavailabilityService = new UserUnavailabilityValidationService(UoW);


            var allReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll();

            var ownerServices = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == userId);

            var reservations = unavailabilityService.FindReservationsForOwnerInPeriod(allReservations, ownerServices, availabilityPeriod.PeriodStart, availabilityPeriod.PeriodEnd);
            var promoActions = unavailabilityService.GetAllPromoActionsForOwnerInPeriod(userId, availabilityPeriod.PeriodStart, availabilityPeriod.PeriodEnd);
            var userAvailabilities = unavailabilityService.GetAllUnavailabilityPeriods(userId, availabilityPeriod.PeriodStart, availabilityPeriod.PeriodEnd);

            bool canPeriodBeAdded = unavailabilityService.CanUnavailabilityPeriodBeAdded(reservations, promoActions, userAvailabilities);

            if (!canPeriodBeAdded)
            {
                return BadRequest("Service owner has scheduled timetable in selected period.");
            }

            UserAvailability availability = availabilityPeriod.ToModel();

            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IUserAvailabilityWriteRepository>().Add(availability);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok("Added availability");
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public void DeleteInstructorAvailabilityPeriod(UserAvailabilityPeriodDTO availabilityPeriod)
        {
            var userId = GetUserIdFromCookie();
            availabilityPeriod.PeriodStart = availabilityPeriod.PeriodStart.ToLocalTime();
            availabilityPeriod.PeriodEnd = availabilityPeriod.PeriodEnd.ToLocalTime();
            var period = UoW.GetRepository<IUserAvailabilityReadRepository>().GetAll()
                .Where(period => period.PeriodStart == availabilityPeriod.PeriodStart && period.PeriodEnd == availabilityPeriod.PeriodEnd)
                .Where(period => period.UserId == userId)
                .FirstOrDefault();
            try
            {
                UoW.BeginTransaction();

                UoW.GetRepository<IUserAvailabilityWriteRepository>().Delete(period);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
            }
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetInstructorServices()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            return Ok(adventures);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult GetServiceAverageMarks()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll();

            var marks = UoW.GetRepository<IMarkReadRepository>()
                .GetAll();

            var users = UoW.GetRepository<IUserReadRepository>().GetAll();

            var userReservations = reservations
                .Join(adventures, r => r.ServiceId, a => a.ServiceId, (r, a) => new { r, a })
                .Join(marks, ra => ra.r.MarkId, m => m.MarkId, (ra, m) => new { ra, m })
                .Join(users, ram => ram.ra.r.UserId, u => u.UserId, (ram, u) => new { ram, u })
                .Select(reservation => new ReservationUserDTO
                {
                    AdditionalEquipment = reservation.ram.ra.r.AdditionalEquipment,
                    IsCanceled = reservation.ram.ra.r.IsCanceled,
                    IsPromo = reservation.ram.ra.r.IsPromo,
                    IsServiceUnavailableMarker = reservation.ram.ra.r.IsServiceUnavailableMarker,
                    MarkId = reservation.ram.ra.r.MarkId,
                    Mark = reservation.ram.m.GivenMark,
                    Price = reservation.ram.ra.r.Price,
                    ReportId = reservation.ram.ra.r.ReportId,
                    ReservationId = reservation.ram.ra.r.ReservationId,
                    ReservedDateTime = reservation.ram.ra.r.ReservedDateTime,
                    ServiceId = reservation.ram.ra.r.ServiceId,
                    ServiceName = reservation.ram.ra.a.Name,
                    UserId = reservation.ram.ra.r.UserId,
                    UsersName = reservation.u.Name,
                    UsersSurname = reservation.u.Surname,
                    UsersPhoneNumber = reservation.u.PhoneNumber,
                    ServiceFrom = reservation.ram.ra.r.StartDateTime,
                    ServiceTo = reservation.ram.ra.r.EndDateTime,
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            var averageMarks = distinctReservations.GroupBy(res => new { ServiceName = res.ServiceName })
               .Select(res => new ServiceAverageMarkDTO
               {
                   AverageMark = res.Average(p => p.Mark).Value,
                   ServiceName = res.Key.ServiceName
               });

            return Ok(averageMarks);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult FilterReservationsByUser(string name, string surname)
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
                });

            var distinctReservations = userReservations
                .GroupBy(res => res.ReservationId)
                .Select(res => res.First());

            var filtered = distinctReservations.Where(reservation => String.IsNullOrEmpty(name) || reservation.UsersName.ToLower().Contains(name.ToLower()))
                                                .Where(reservation => String.IsNullOrEmpty(surname) || reservation.UsersSurname.ToLower().Contains(surname.ToLower()));

            return Ok(filtered);
        }


        private Report CreateNewReport(ReportDTO report)
        {
            return new Report()
            {
                CreatedDateTime = DateTime.Now,
                ReportText = report.ReportText,
                ShownUp = report.ShownUp,
            };
        }

    }
}
