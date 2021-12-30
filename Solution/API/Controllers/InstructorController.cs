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
                .Where(x => x.InstructorId == ownerId).FirstOrDefault();

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
        public IActionResult GetReservationsWithBasicUserInfo()
        {
            int ownerId = GetUserIdFromCookie();

            var adventures = UoW.GetRepository<IServiceReadRepository>()
               .GetAll()
               .Where(x => x.OwnerId == ownerId);

            var bla = adventures.Select(y => y.ServiceId).ToArray();
            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId.In(bla));



            IEnumerable<ReservationUserDTO> userReservations = MapReservationsAndUsers(reservations);

            foreach (var reservation in userReservations)
            {
                var adventure = UoW.GetRepository<IServiceReadRepository>().GetById(reservation.ServiceId);
                reservation.ServiceName = adventure.Name;
                reservation.Capacity = adventure.Capacity;
            }

            if (!userReservations.Any())
            {
                return NotFound();
            }

            return Ok(userReservations);
        }


        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Instructor })]
        public IActionResult UpdateAdditionalInstructorInfo(AdditionalInstructorInfoDTO availability)
        {
            var userId = GetUserIdFromCookie();

            var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetAll()
                .Where(x => x.InstructorId == userId).FirstOrDefault();

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
                .Where(x => x.InstructorId == userId).FirstOrDefault();


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


        private Report CreateNewReport(ReportDTO report)
        {
            return new Report()
            {
                CreatedDateTime = DateTime.Now,
                ReportText = report.ReportText
            };
        }

        private IEnumerable<ReservationUserDTO> MapReservationsAndUsers(IEnumerable<Reservation> reservations)
        {
            List<ReservationUserDTO> userReservations = new List<ReservationUserDTO>();
            var usersReadRepository = UoW.GetRepository<IUserReadRepository>();

            foreach(var reservation in reservations)
            {
                User user = usersReadRepository.GetById(reservation.UserId);
                ReservationUserDTO dto = new ReservationUserDTO
                {
                    AdditionalEquipment = reservation.AdditionalEquipment,
                    IsCanceled = reservation.IsCanceled,
                    IsPromo = reservation.IsPromo,
                    IsServiceUnavailableMarker = reservation.IsServiceUnavailableMarker,
                    MarkId = reservation.MarkId,
                    Price = reservation.Price,
                    ReportId = reservation.ReportId,
                    ReservationId = reservation.ReservationId,
                    ReservedDateTime = reservation.ReservedDateTime,
                    ServiceId = reservation.ServiceId,
                    UserId = reservation.UserId,
                    UsersName = user.Name,
                    UsersSurname = user.Surname,
                    UsersPhoneNumber = user.PhoneNumber,
                    ServiceFrom = reservation.StartDateTime,
                    ServiceTo = reservation.EndDateTime,
                    
                };
                userReservations.Add(dto);
            }


            return userReservations;

        }
    }
}
