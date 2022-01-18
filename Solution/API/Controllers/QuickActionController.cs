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
using Services;
using Services.Constants;
using Services.HtmlWriter;
using Services.Validators;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuickActionController : AdvancedController
    {
        public QuickActionController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        public IEnumerable<PromoActionWrapperDTO> GetQuickActions(int serviceId)
        {
            var promoActionReadRepository = UoW.GetRepository<IPromoActionReadRepository>();

            return promoActionReadRepository.GetAll()
                .Where(x => x.ServiceId == serviceId)
                .Select(x => new PromoActionWrapperDTO()
                {
                    PromoActionId = x.PromoActionId,
                    ServiceId = x.ServiceId,
                    PricePerDay = x.PricePerDay,
                    IsTaken = x.IsTaken,
                    Capacity = x.Capacity,
                    AddedBenefits = x.AddedBenefits,
                    Place = x.Place,
                    Role = GetRole(x.PromoActionId, true),
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });
        }

        [HttpGet]
        public IEnumerable<PromoAction> GetUsersQuickActions()
        {
            var userId = GetUserIdFromCookie();
            var promoActionReadRepository = UoW.GetRepository<IPromoActionReadRepository>();
            var services = UoW.GetRepository<IServiceReadRepository>().GetAll()
                .Where(service => service.OwnerId == userId);
                //.Select(service => new { service.ServiceId });

            return promoActionReadRepository.GetAll()
                .Where(action => services.Any(service => service.ServiceId == action.ServiceId));
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult CreateNewQuickAction(QuickActionDTO newAction)
        {
            if (!CheckOwnerShip(newAction.ServiceId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            UoW.BeginTransaction();

            var service = new Service();
            try
            {
                service = new ServiceLocker(UoW).ObtainLockedService(newAction.ServiceId);
            }
            catch
            {
                return BadRequest(Responses.UnavailableRightNow);
            }

            if (service.AvailableTo != null && service.AvailableFrom != null)
            {
                if (!(service.AvailableFrom <= newAction.StartDateTime &&
                      service.AvailableTo >= newAction.EndDateTime))
                {
                    return BadRequest(Responses.ServiceNotAvailableAtGivenTime);
                }
            }

            if (service.ServiceType == ServiceType.Boat && newAction.IsCaptain == null)
            {
                return BadRequest(Responses.MissingResponsibility);
            }

            var dateToCheck = new CalendarItem()
            {
                StartDateTime = newAction.StartDateTime,
                EndDateTime = newAction.EndDateTime
            };

            var union = GetRelevantDateIntervalsForService(newAction.ServiceId);

            var overlaps = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(dateToCheck, union);
            if (overlaps)
            {
                return BadRequest(Responses.DatesOverlap);
            }

            var action = CreateNewAction(newAction);

            try
            {
                UoW.GetRepository<IPromoActionWriteRepository>().Add(action);

                if (service.ServiceType == ServiceType.Boat)
                {
                    var boatOwnerResp = new BoatReservationDetail()
                    {
                        BoatOwnerResponsibilityType = newAction.IsCaptain.Value
                            ? BoatOwnerResponsibilityType.Captain
                            : BoatOwnerResponsibilityType.FirstAssistant,
                        IsPromo = true,
                        RelevantId = action.PromoActionId
                    };
                    UoW.GetRepository<IBoatReservationDetailWriteRepository>().Add(boatOwnerResp);
                }

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            NotifyAllSubscribedUsers(action);

            return Ok(Responses.Ok);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeServiceOwnerAttribute))]
        public IActionResult UpdateQuickAction(QuickActionDTO action)
        {
            if (action.PromoActionId == null)
            {
                ModelState.AddModelError("PromoActionId", "The action to be updated is not specified!");
                return BadRequest(ModelState);
            }

            if (!CheckOwnerShip(action.ServiceId))
            {
                return Unauthorized(Responses.ServiceOwnerNotLinked);
            }

            var relevantAction = UoW.GetRepository<IPromoActionReadRepository>().GetById(action.PromoActionId.Value);
            if (relevantAction == null)
            {
                return NotFound(Responses.NotFound);
            }
            else if (relevantAction.IsTaken)
            {
                return BadRequest(Responses.PromoActionTaken);
            }

            UoW.BeginTransaction();

            var service = new Service();
            try
            {
                service = new ServiceLocker(UoW).ObtainLockedService(action.ServiceId);
            }
            catch
            {
                return BadRequest(Responses.UnavailableRightNow);
            }

            if (service.ServiceType == ServiceType.Boat && action.IsCaptain == null)
            {
                return BadRequest(Responses.MissingResponsibility);
            }

            if (service.AvailableTo != null && service.AvailableFrom != null)
            {
                if (!(service.AvailableFrom <= action.StartDateTime &&
                      service.AvailableTo >= action.EndDateTime))
                {
                    return BadRequest(Responses.ServiceNotAvailableAtGivenTime);
                }
            }

            if (action.StartDateTime != relevantAction.StartDateTime ||
                action.EndDateTime != relevantAction.EndDateTime)
            {
                var dateToCheck = new CalendarItem()
                {
                    StartDateTime = action.StartDateTime,
                    EndDateTime = action.EndDateTime
                };

                var union = GetRelevantDateIntervalsForService(action.ServiceId);
                union = union.Where(x => x.StartDateTime != relevantAction.StartDateTime && x.EndDateTime != relevantAction.EndDateTime);

                var overlaps = new ValidateReservationDatesService().CalendarItemOverlapsWithAny(dateToCheck, union);
                if (overlaps)
                {
                    return BadRequest(Responses.DatesOverlap);
                }
            }

            MapToExistingAction(action, relevantAction);

            try
            {
                UoW.GetRepository<IPromoActionWriteRepository>().Update(relevantAction);

                if (service.ServiceType == ServiceType.Boat)
                {
                    var foundResp = UoW.GetRepository<IBoatReservationDetailReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.IsPromo && x.RelevantId == action.PromoActionId);
                    foundResp.BoatOwnerResponsibilityType = action.IsCaptain.Value
                        ? BoatOwnerResponsibilityType.Captain
                        : BoatOwnerResponsibilityType.FirstAssistant;
                    UoW.GetRepository<IBoatReservationDetailWriteRepository>()
                        .Update(foundResp);
                } 

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            NotifyAllSubscribedUsers(relevantAction);

            return Ok(Responses.Ok);
        }

        private PromoAction CreateNewAction(QuickActionDTO action)
        {
            return new PromoAction()
            {
                ServiceId = action.ServiceId,
                PricePerDay = action.PricePerDay,
                IsTaken = false,
                Capacity = action.Capacity,
                AddedBenefits = action.AddedBenefits,
                StartDateTime = action.StartDateTime,
                EndDateTime = action.EndDateTime,
                Place = action.Place
            };
        }

        private IEnumerable<CalendarItem> GetRelevantDateIntervalsForService(int serviceId)
        {
            var existingActionsForService = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            var existingReservationsForService = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == serviceId && !x.IsCanceled && x.EndDateTime > DateTime.Now)
                .Select(x => new CalendarItem()
                {
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                });

            return existingActionsForService.Union(existingReservationsForService);
        }

        private void MapToExistingAction(QuickActionDTO dto, PromoAction action)
        {
            action.PricePerDay = dto.PricePerDay;
            action.Capacity = dto.Capacity;
            action.AddedBenefits = dto.AddedBenefits;
            action.StartDateTime = dto.StartDateTime;
            action.EndDateTime = dto.EndDateTime;
        }

        private void NotifyAllSubscribedUsers(PromoAction action)
        {
            var service = UoW.GetRepository<IServiceReadRepository>().GetById(action.ServiceId);

            var mailBody = HtmlWriter.PromoActionNotificationTemplate(service.Name, action);

            var subscriptions = UoW.GetRepository<ISubscriptionReadRepository>()
                .GetAll()
                .Where(x => x.ServiceId == action.ServiceId)
                .Select(x => x.UserId);

            if (subscriptions.Any())
            {
                var users = UoW.GetRepository<IUserReadRepository>()
                    .GetAll()
                    .Where(x => x.UserId.In(subscriptions.ToArray()));
                foreach (var user in users)
                {
                    var mailingService = new MailingService(UoW)
                    {
                        Body = mailBody,
                        Receiver = user.Email,
                        Title = "Info"
                    };
                    mailingService.Send();
                }
            }
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
