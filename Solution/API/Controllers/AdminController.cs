using API.Attributes;
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
                    .FirstOrDefault(x => x.UserId == userId);

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

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult UpdateMoneyPercentageSystemTakes(MoneyPercentageSystemMakesDTO systemConfigDTO)
        {
            double moneyPercentage;
            bool moneyValid = Double.TryParse(systemConfigDTO.Value, out moneyPercentage);
            if (!moneyValid)
            {
                return BadRequest("Invalid money format.");
            }

            var systemConfig = UoW.GetRepository<ISystemConfigReadRepository>().GetAll()
                .Where(x => x.Name == "MoneyPercentageSystemTakes").FirstOrDefault();

            if(systemConfig == null)
            {
                try
                {
                    UoW.BeginTransaction();

                    systemConfig = new SystemConfig
                    {
                        Name = "MoneyPercentageSystemTakes",
                        Value = systemConfigDTO.Value,
                    };

                    UoW.GetRepository<ISystemConfigWriteRepository>().Add(systemConfig);
                    UoW.Commit();
                }
                catch (Exception e)
                {
                    UoW.Rollback();
                    return BadRequest();
                }
            }
            else
            {
                try
                {
                    systemConfig.Value = systemConfigDTO.Value;

                    UoW.BeginTransaction();
                    UoW.GetRepository<ISystemConfigWriteRepository>().Update(systemConfig);
                    UoW.Commit();
                }
                catch(Exception e)
                {
                    UoW.Rollback();
                    return BadRequest();
                }
            }

            return Ok(systemConfig);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetMoneyPercentageSystemTakes()
        {
            var systemConfig = UoW.GetRepository<ISystemConfigReadRepository>().GetAll()
                .Where(x => x.Name == "MoneyPercentageSystemTakes").FirstOrDefault();

            if(systemConfig == null)
            {
                return NotFound();
            }

            return Ok(systemConfig);
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetReservationRevenue()
        {
            var reservations = UoW.GetRepository<IReservationReadRepository>().GetAll()
                .Where(x => !x.IsCanceled);
                //.Where(x => x.EndDateTime < DateTime.Now);

            var services = UoW.GetRepository<IServiceReadRepository>().GetAll();
            var users = UoW.GetRepository<IUserReadRepository>().GetAll();


            var reservationRevenueInfos = reservations
                .Join(services, r => r.ServiceId, s => s.ServiceId, (r, s) => new { r, s })
                .Join(users, rs => rs.r.UserId, u => u.UserId, (rs, u) => new { rs, u })
                .Select(revenue => new ReservationRevenueDTO
                {
                    ServiceId = revenue.rs.s.ServiceId,
                    Price = revenue.rs.r.Price,
                    ReservationId = revenue.rs.r.ReservationId,
                    ServiceName = revenue.rs.s.Name,
                    ServiceType = revenue.rs.s.ServiceType,
                    ServiceStart = revenue.rs.r.StartDateTime,
                    ServiceEnd = revenue.rs.r.EndDateTime,
                    UserId = revenue.u.UserId,
                    UserName = revenue.u.Name,
                    UserSurname = revenue.u.Surname,
                });

            return Ok(reservationRevenueInfos);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult ChangePassword(PasswordChangeDTO passwordChangeDto)
        {
            var userId = GetUserIdFromCookie();
            var user = UoW.GetRepository<IUserReadRepository>().GetById(userId);

            if (user.Password != passwordChangeDto.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "The password doesn't match your actual password!");
                return BadRequest(ModelState);
            }

            user.Password = passwordChangeDto.NewPassword;

            try
            {
                UoW.BeginTransaction();

                UoW.GetRepository<IUserWriteRepository>().Update(user);

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
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetUserInfo()
        {
            var userId = GetUserIdFromCookie();

            var userInfo = UoW.GetRepository<IUserReadRepository>().GetById(userId);

            return Ok(userInfo);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult ActivateAdmin(User admin)
        {
            var userId = GetUserIdFromCookie();
            var oldAdmin = UoW.GetRepository<IUserReadRepository>()
                .GetAll()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();


            if (admin.UserId != userId || oldAdmin == null)
            {
                return BadRequest("Cannot activate.");
            }

            oldAdmin.IsAccountActive = true;
            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IUserWriteRepository>().Update(oldAdmin);
                UoW.Commit();
            }
            catch(Exception e)
            {
                UoW.Rollback();
                return BadRequest("Activation failed.");
            }
            
            return Ok();
        }
    }
}
