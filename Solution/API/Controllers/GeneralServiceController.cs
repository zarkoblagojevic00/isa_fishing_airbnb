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
using Domain.Entities.Abstractions;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentNHibernate.Utils;
using Services;
using Services.Constants;
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
                .Where(x => x.ServiceId == serviceId);

            var marks = UoW.GetRepository<IMarkReadRepository>()
                .GetAll()
                .Where(x => x.MarkId.In(reservationHistory
                    .Where(x => x.MarkId != null)
                    .Select(x => x.MarkId.Value)
                    .Distinct()
                    .ToArray()));
            var reports = UoW.GetRepository<IReportReadRepository>()
                .GetAll()
                .Where(x => x.ReportId.In(reservationHistory
                    .Where(x => x.ReportId != null)
                    .Select(x => x.ReportId.Value)
                    .Distinct()
                    .ToArray()));

            var result = reservationHistory.Join(marks, x => x.MarkId, y => y.MarkId, (x, y) =>
                new ReservationHistoryDTO()
                {
                    Reservation = x,
                    Mark = y
                }).Join(reports, x => x.Reservation.ReportId, y => y.ReportId, (x, y) =>
                {
                    x.Report = y;
                    return x;
                });

            return Ok(result);
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

            return Ok(user);
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
                .FirstOrDefault(x => x.Email == reservationDto.UserMail);
            if (user == null)
            {
                return BadRequest(Responses.NotFound);
            }

            if (!CheckOwnerShip(reservationDto.ServiceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var userReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.UserId == user.UserId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var serviceReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == reservationDto.ServiceId && !x.IsCanceled && x.EndDateTime > DateTime.Now);

            var union = userReservations.Union(serviceReservations);
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
                UoW.BeginTransaction();

                var newReservation = CreateNewReservation(reservationDto, user.UserId);

                UoW.GetRepository<IReservationWriteRepository>().Add(newReservation);

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
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult GetCurrentReservations(int serviceId)
        {
            if (!CheckOwnerShip(serviceId))
            {
                return BadRequest(Responses.ServiceOwnerNotLinked);
            }

            var ongoingReservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime < DateTime.Now)
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

        private Report CreateNewReport(ReportDTO report)
        {
            return new Report()
            {
                CreatedDateTime = DateTime.Now,
                ReportText = report.ReportText
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
    }
}