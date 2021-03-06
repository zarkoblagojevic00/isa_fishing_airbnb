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
                .Where(x => !x.IsCanceled)
                .Where(res => res.EndDateTime < DateTime.Now);

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
                    AdditionalEquipment = revenue.rs.r.AdditionalEquipment,
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
            MarkLocker markLocker = new MarkLocker(UoW);
            Mark oldMark;

            try
            {
                UoW.BeginTransaction();

                try
                {
                    oldMark = markLocker.ObtainLockedMark(mark.MarkId);
                }
                catch
                {
                    return BadRequest(Responses.UnavailableRightNow);
                }

                if (oldMark == null || oldMark.IsReviewed)
                {
                    return BadRequest("Cannot interract with mark request.");
                }

                
                oldMark.IsReviewed = true;
                oldMark.IsApproved = true;
                UoW.GetRepository<IMarkWriteRepository>().Update(oldMark);

                var mailService = new MailingService(UoW)
                {
                    Body = GenerateMarkReviewMailBody(mark),
                    Receiver = mark.Email,
                    Title = "Mark review"
                };
                mailService.Send();

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
        public IActionResult DeclineMarkRequest(UserMarkInformationDTO mark)
        {
            MarkLocker markLocker = new MarkLocker(UoW);
            Mark oldMark;
            try
            {
                UoW.BeginTransaction();
                try
                {
                    oldMark = markLocker.ObtainLockedMark(mark.MarkId);
                }
                catch
                {
                    return BadRequest(Responses.UnavailableRightNow);
                }

                if(oldMark == null || oldMark.IsReviewed)
                {
                    return BadRequest("Cannot interract with mark request.");
                }

                
                oldMark.IsReviewed = true;
                oldMark.IsApproved = false;
                UoW.GetRepository<IMarkWriteRepository>().Update(oldMark);
                UoW.Commit();

            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Review failed.");
            }
            return Ok("Mark reviewed");

        }


        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult RespondToIssue(IssueInformationDTO issue)
        {
            Issue oldIssue;
            var issueLocker = new IssueLocker(UoW);
            try
            {
                UoW.BeginTransaction();
                try
                {
                    oldIssue = issueLocker.ObtainLockedIssue(issue.IssueId);
                }
                catch
                {
                    return BadRequest(Responses.UnavailableRightNow);
                }

                if (oldIssue == null || oldIssue.IsReviewed)
                {
                    return BadRequest("Issue not found.");
                }

                oldIssue.IsReviewed = true;

                UoW.GetRepository<IIssueWriteRepository>().Update(oldIssue);

                var mailService = new MailingService(UoW)
                {
                    Body = GenerateIssueReviewMailBody(issue),
                    Receiver = issue.ServiceOwnerEmail,
                    Title = "Issue review"
                };
                mailService.Send();
                mailService.Receiver = issue.UserEmail;
                mailService.Send();

                UoW.Commit();
                
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Issue review failed.");
            }
            return Ok("Issue reviewed");

        }

        [HttpPut]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult RespondAccountDeletionRequest(AccountDeletionRequestViewDTO request)
        {
            var adminService = new AdminRequestsService(UoW);

            try
            {
                UoW.BeginTransaction();
                AccountDeletionRequest oldRequest = new AccountDeletionRequest();
                try
                {
                    oldRequest = new AccountDeletionRequestLocker(UoW).ObtainLockedAccountDeletionRequest(request.UserId);
                }
                catch
                {
                    return BadRequest(Responses.UnavailableRightNow);
                }

                if (oldRequest.IsReviewed)
                {
                    return BadRequest("Cannot interract with user right now.");
                }


                oldRequest.IsReviewed = true;
                oldRequest.IsApproved = request.IsApproved;

                if (request.IsApproved)
                {
                    bool canBeDeleted = adminService.CanUserBeDeleted(request.UserId);
                    
                    if (!canBeDeleted)
                    {
                        return BadRequest(Responses.UserCannotBeDeleted);
                    }

                    var user = UoW.GetRepository<IUserReadRepository>().GetById(request.UserId);
                    adminService.DeleteRequestedUser(user);

                }
                
                UoW.GetRepository<IAccountDeletionRequestWriteRepository>().Update(oldRequest);

                UoW.Commit();

                var mailService = new MailingService(UoW)
                {
                    Body = GenerateAccountDeletionRequestMailBody(request),
                    Receiver = request.Email,
                    Title = "Account deletion review"
                };
                mailService.Send();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return BadRequest("Account deletion review failed.");
            }
            return Ok("Account deletion reviewed");

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

            var serviceOwners = UoW.GetRepository<IUserReadRepository>().GetAll()
                .Where(u => u.UserType != UserType.Admin && u.UserType != UserType.Registered);

            var clients = UoW.GetRepository<IUserReadRepository>().GetAll()
                .Where(u => u.UserType == UserType.Registered);

            var userIssueInformationServiceOwner = issues
                .Join(clients, ii => ii.IssuedEntityId, s => s.UserId, (ii, s) => new { ii, s })
                .Join(serviceOwners, iis => iis.ii.UserId, u => u.UserId, (iis, u) => new { iis, u })
                .Select(information => new IssueInformationDTO
                {
                    IssueId = information.iis.ii.IssueId,
                    IssuedEntityId = information.iis.s.UserId,
                    UserName = information.u.Name,
                    UserSurname = information.u.Surname,
                    UserEmail = information.u.Email,
                    ServiceOwnerName = information.iis.s.Name,
                    ServiceOwnerSurname = information.iis.s.Surname,
                    ServiceOwnerEmail = information.iis.s.Email,
                    Reason = information.iis.ii.Reason,
                    Response = "",
                    CreatedDateTime = information.iis.ii.CreatedDateTime,
                    IsReviewed = information.iis.ii.IsReviewed,
                });

            return Ok(userIssueInformation.Concat(userIssueInformationServiceOwner));
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAdminAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult GetDeletionRequests()
        {
            var requests = UoW.GetRepository<IAccountDeletionRequestReadRepository>().GetAll()
                .Where(request => !request.IsReviewed);
            var users = UoW.GetRepository<IUserReadRepository>().GetAll();

            var requestInformation = requests
                .Join(users, r => r.UserId, u => u.UserId, (r, u) => new { r, u })
                .Select(request => new AccountDeletionRequestViewDTO
                {
                    Name = request.u.Name,
                    Surname = request.u.Surname,
                    Email = request.u.Email,
                    Reason = request.r.Reason,
                    IsReviewed = request.r.IsReviewed,
                    UserId = request.u.UserId,
                    IsApproved = false
                });

            return Ok(requestInformation);
        }


        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult CalculateFinishedReservationsRevenue(RevenueRangeDTO revenueRange)
        {
            if (revenueRange == null)
            {
                return BadRequest("Bad revenue range");
            }

            revenueRange.Start = revenueRange.Start.ToLocalTime();
            revenueRange.End = revenueRange.End.ToLocalTime();

            var services = UoW.GetRepository<IServiceReadRepository>()
               .GetAll();

            var reservations = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(res => res.EndDateTime < revenueRange.End && res.EndDateTime > revenueRange.Start && !res.IsCanceled);

            var systemConfig = UoW.GetRepository<ISystemConfigReadRepository>()
                .GetAll()
                .Where(con => con.Name == "MoneyPercentageSystemTakes")
                .FirstOrDefault();

            if(systemConfig == null)
            {
                return BadRequest("Money percentage system takes not set.");
            }

            double percentageSystemTakes;
            bool success = double.TryParse(systemConfig.Value, out percentageSystemTakes);


            if (!success)
            {
                return BadRequest("Money percentage system takes not set.");
            }


            double totalPrice = 0;
            foreach (var reservation in reservations)
            {
                var service = services.Where(ser => ser.ServiceId == reservation.ServiceId)
                    .FirstOrDefault();
                if (service == null)
                    continue;

                TimeSpan span = reservation.EndDateTime.Subtract(reservation.StartDateTime);

                double spanDuration = 0;
                if (service.ServiceType == ServiceType.Adventure)
                    spanDuration = span.TotalHours;
                else
                    spanDuration = span.TotalDays;

                var parsedEquipment = ParseAdditionalEquipment(reservation.AdditionalEquipment);
                totalPrice += reservation.Price * spanDuration;
                foreach (var eq in parsedEquipment)
                {
                    totalPrice += eq.Price * spanDuration;
                }
            }
            return Ok(totalPrice * percentageSystemTakes * 0.01);
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

        private string GenerateAccountDeletionRequestMailBody(AccountDeletionRequestViewDTO request)
        {
            var stringBuilder = new StringBuilder();

            if(request.IsApproved)
                stringBuilder.Append("Your account deletion request has been reviewed and accepted!").Append(HtmlWriter.Br());
            else
                stringBuilder.Append("Your account deletion request has been reviewed and declined!").Append(HtmlWriter.Br());
            stringBuilder.Append(HtmlWriter.Br());
            if (!request.IsApproved)
            {
                stringBuilder.Append(request.Response);
                stringBuilder.Append(HtmlWriter.Br());
            }

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

        private List<AdditionalEquipmentDTO> ParseAdditionalEquipment(string additionalEquipment)
        {
            List<AdditionalEquipmentDTO> additionalEquipmentArray = new List<AdditionalEquipmentDTO>();
            if (additionalEquipment == null || additionalEquipment == "")
            {
                return additionalEquipmentArray;
            }

            var receivedEq = additionalEquipment.Split(";");
            foreach (var eq in receivedEq)
            {
                if (eq == "" || eq == null)
                {
                    continue;
                }
                string name = eq.Split(":")[0];
                string price = eq.Split(":")[1];

                double priceVal;
                bool success = double.TryParse(price, out priceVal);


                if (!success || name == "" || price == "")
                {
                    continue;
                }
                additionalEquipmentArray.Add(new AdditionalEquipmentDTO
                {
                    Name = name,
                    Price = priceVal,
                });
            }

            return additionalEquipmentArray;
        }
    }
}
