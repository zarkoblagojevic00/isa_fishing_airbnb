﻿using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
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
    public class AdminController : AdvancedController
    {
        public AdminController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult DeleteService(int serviceId)
        {

            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == serviceId && !x.IsCanceled);

            var promoActionDates = UoW.GetRepository<IPromoActionReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.ServiceId == serviceId && x.IsTaken);

            if (reservationDates.Any() || promoActionDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            try
            {

                UoW.BeginTransaction();

                var additionalAdventureInfo = UoW.GetRepository<IAdditionalAdventureInfoReadRepository>().GetAll()
                    .FirstOrDefault(x => x.AdventureId == serviceId);

                var additionalBoatServiceInfo = UoW.GetRepository<IAdditionalBoatServiceInfoReadRepository>().GetAll()
                    .FirstOrDefault(x => x.ServiceId == serviceId);

                var additionalVillaServiceInfo = UoW.GetRepository<IAdditionalVillaServiceInfoReadRepository>().GetAll()
                    .FirstOrDefault(x => x.ServiceId == serviceId);

                var service = UoW.GetRepository<IServiceReadRepository>().GetById(serviceId);

                var promoActions = UoW.GetRepository<IPromoActionReadRepository>()
                    .GetAll()
                    .Where(x => x.ServiceId == serviceId);
                foreach (var promoAction in promoActions)
                {
                    promoAction.IsTaken = true;
                    UoW.GetRepository<IPromoActionWriteRepository>().Update(promoAction);
                }

                if(additionalAdventureInfo != null)
                    UoW.GetRepository<IAdditionalAdventureInfoWriteRepository>().Delete(additionalAdventureInfo);

                if (additionalBoatServiceInfo != null)
                    UoW.GetRepository<IAdditionalBoatServiceInfoWriteRepository>().Delete(additionalBoatServiceInfo);

                if (additionalVillaServiceInfo != null)
                    UoW.GetRepository<IAdditionalVillaServiceInfoWriteRepository>().Delete(additionalVillaServiceInfo);

                UoW.GetRepository<IServiceWriteRepository>().Delete(service);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest();
            }

            return Ok(Responses.Ok);
        }

        [HttpDelete]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult DeleteUser(int userId)
        {
            // note- promoaction missing user that reserves it
            var reservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.UserId == userId && !x.IsCanceled);


            if (reservationDates.Any())
            {
                return BadRequest(Responses.CannotChangeService);
            }

            try
            {

                UoW.BeginTransaction();

                var additionalInstructorInfo = UoW.GetRepository<IAdditionalInstructorInfoReadRepository>().GetAll()
                    .FirstOrDefault(x => x.InstructorId == userId);

                var user = UoW.GetRepository<IUserReadRepository>().GetById(userId);

                if(user.UserType == UserType.Admin)
                {
                    UoW.Rollback();
                    return BadRequest("Cannot remove admin.");
                }

                if(additionalInstructorInfo != null)
                    UoW.GetRepository<IAdditionalInstructorInfoWriteRepository>().Delete(additionalInstructorInfo);

                UoW.GetRepository<IUserWriteRepository>().Delete(user);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest();
            }

            return Ok(Responses.Ok);
        }
    }
}
