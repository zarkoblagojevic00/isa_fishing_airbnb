using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Constants;
using Services.HtmlWriter;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [HttpGet]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetUnapprovedMarks()
        {
            var services = UoW.GetRepository<IServiceReadRepository>().GetAll();
            var users = UoW.GetRepository<IUserReadRepository>().GetAll();
            var marks = UoW.GetRepository<IMarkReadRepository>().GetAll()
                .Where(mark => !mark.IsReviewed);

            var userMarkInformation = marks
                .Join(services, m => m.ServiceId, s => s.ServiceId, (m, s) => new { m, s })
                .Join(users, ms => ms.m.UserId, u => u.UserId, (ms, u) => new { ms, u })
                .Join(users, msu => msu.ms.s.OwnerId, us => us.UserId, (msu, us) => new { msu, us })
                .Select(information => new UserMarkInformationDTO
                {
                    UserName = information.msu.u.Name,
                    UserSurname = information.msu.u.Surname,
                    Email = information.us.Email,
                    ServiceName = information.msu.ms.s.Name,
                    Mark = information.msu.ms.m.GivenMark,
                    Description = information.msu.ms.m.Description,
                    IsApproved = information.msu.ms.m.IsApproved,
                    MarkId = information.msu.ms.m.MarkId
                });

            return Ok(userMarkInformation);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult ApproveMarkRequest(UserMarkInformationDTO mark)
        {
            var oldMark = UoW.GetRepository<IMarkReadRepository>().GetById(mark.MarkId);
            oldMark.IsReviewed = true;
            oldMark.IsApproved = true;

            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IMarkWriteRepository>().Update(oldMark);
                UoW.Commit();

                var mailService = new MailingService(UoW)
                {
                    Body = GenerateMarkReviewMailBody(mark),
                    Receiver = mark.Email,
                    Title = "Mark review"
                };
                mailService.Send();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Approval failed.");
            }
            return Ok("Mark reviewed");

        }

        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult DeclineMarkRequest(UserMarkInformationDTO mark)
        {
            var oldMark = UoW.GetRepository<IMarkReadRepository>().GetById(mark.MarkId);
            oldMark.IsReviewed = true;
            oldMark.IsApproved = false;

            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IMarkWriteRepository>().Update(oldMark);
                UoW.Commit();

            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Approval failed.");
            }
            return Ok("Mark reviewed");

        }


        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult RespondToIssue(IssueInformationDTO issue)
        {
            var oldIssue = UoW.GetRepository<IIssueReadRepository>().GetById(issue.IssueId);
            oldIssue.IsReviewed = true;


            try
            {
                UoW.BeginTransaction();
                UoW.GetRepository<IIssueWriteRepository>().Update(oldIssue);
                UoW.Commit();
                var mailService = new MailingService(UoW)
                {
                    Body = GenerateIssueReviewMailBody(issue),
                    Receiver = issue.UserEmail,
                    Title = "Issue review"
                };
                mailService.Send();
                mailService.Receiver = issue.ServiceOwnerEmail;
                mailService.Send();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Issue review failed.");
            }
            return Ok("Issue reviewed");

        }


        [HttpGet]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetUnapprovedIssues()
        {
            var services = UoW.GetRepository<IServiceReadRepository>().GetAll();
            var users = UoW.GetRepository<IUserReadRepository>().GetAll();
            var issues = UoW.GetRepository<IIssueReadRepository>().GetAll()
                .Where(issue => !issue.IsReviewed);

            var userIssueInformation = issues
                .Join(services, ii => ii.IssuedEntityId, s => s.ServiceId, (ii, s) => new { ii, s })
                .Join(users, iis => iis.ii.UserId, u => u.UserId, (iis, u) => new { iis, u })
                .Join(users, iisu => iisu.iis.s.OwnerId, us => us.UserId, (iisu, us) => new { iisu, us })
                .Select(information => new IssueInformationDTO
                {
                    IssueId = information.iisu.iis.ii.IssueId,
                    IssuedEntityId = information.iisu.iis.s.ServiceId,
                    UserName = information.iisu.u.Name,
                    UserSurname = information.iisu.u.Surname,
                    UserEmail = information.iisu.u.Email,
                    ServiceOwnerName = information.us.Name,
                    ServiceOwnerSurname = information.us.Surname,
                    ServiceOwnerEmail = information.us.Email,
                    Reason = information.iisu.iis.ii.Reason,
                    Response = "",
                    CreatedDateTime = information.iisu.iis.ii.CreatedDateTime,
                    IsReviewed = information.iisu.iis.ii.IsReviewed,
                });

            return Ok(userIssueInformation);
        }


        private string GenerateMarkReviewMailBody(UserMarkInformationDTO mark)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Your mark request has been reviewed and accepted!").Append(HtmlWriter.Br());
            stringBuilder.Append(HtmlWriter.Br());

            stringBuilder.Append(mark.Description);
            stringBuilder.Append(HtmlWriter.Br());
            stringBuilder.Append("Given mark: " + mark.Mark.ToString());



            return stringBuilder.ToString();
        }

        private string GenerateIssueReviewMailBody(IssueInformationDTO issue)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Issuer: " + issue.UserEmail);
            stringBuilder.Append(HtmlWriter.Br());
            stringBuilder.Append("Issuee: " + issue.ServiceOwnerEmail);
            stringBuilder.Append(HtmlWriter.Br());
            stringBuilder.Append("Issue text: " + issue.Reason);
            stringBuilder.Append(HtmlWriter.Br());
            stringBuilder.Append("Admin response: " + issue.Response);
            stringBuilder.Append(HtmlWriter.Br());




            return stringBuilder.ToString();
        }
    }
}
