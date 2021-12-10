using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Attributes;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Services;
using Services.Constants;
using Services.HtmlWriter;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegistrationReviewController : AdvancedController
    {
        public RegistrationReviewController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[]{false, UserType.Admin})]
        public IEnumerable<UserAndRegistrationReasonDTO> GetAll()
        {
            var registrationReasonRepo = UoW.GetRepository<IRegistrationReasonReadRepository>();
            var userRepo = UoW.GetRepository<IUserReadRepository>();

            var allRequests = registrationReasonRepo.GetAll();
            var allUsers = userRepo.GetAll();
            var allCities = UoW.GetRepository<ICityReadRepository>().GetAll();
            var allCountries = UoW.GetRepository<ICountryReadRepository>().GetAll();

            return allRequests
                .Join(allUsers, x => x.UserId, y => y.UserId, (x, y) => new UserAndRegistrationReasonDTO()
                {
                    UserId = y.UserId,
                    Reason = x.Reason,
                    DenialReason = x.DenialReason,
                    IsReviewed = x.IsReviewed,
                    UserType = y.UserType,
                    Name = y.Name,
                    Surname = y.Surname,
                    Address = y.Address,
                    CityId = y.CityId,
                    PhoneNumber = y.PhoneNumber,
                    Email = y.Email,
                    IsAccountActive = y.IsAccountActive,
                    IsAccountVerified = y.IsAccountVerified
                })
                .Join(allCities, x => x.CityId, y => y.CityId, (x, y) =>
                {
                    x.City = y.Name;
                    x.CountryId = y.CountryId;
                    return x;
                })
                .Join(allCountries, x => x.CountryId, y => y.CountryId, (x, y) =>
                {
                    x.Country = y.Name;
                    return x;
                });
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { false, UserType.Admin })]
        public IActionResult ReviewRequest(RegistrationReviewDTO reviewDto)
        {
            if (!reviewDto.Result && string.IsNullOrEmpty(reviewDto.Reason))
            {
                ModelState.AddModelError("Reason", "If the user is denied access the reason should be specified");
                return BadRequest(ModelState);
            }
            
            var registrationReasonReadRepo = UoW.GetRepository<IRegistrationReasonReadRepository>();
            var registrationReasonWriteRepo = UoW.GetRepository<IRegistrationReasonWriteRepository>();
            var userWriteRepo = UoW.GetRepository<IUserWriteRepository>();
            var userReadRepo = UoW.GetRepository<IUserReadRepository>();

            var regReason = registrationReasonReadRepo.GetAll()
                .FirstOrDefault(x => x.UserId == reviewDto.UserId);

            if (regReason == null)
            {
                ModelState.AddModelError("UserId", "The registration request was not found");
                return BadRequest(ModelState);
            }

            UoW.BeginTransaction();

            try
            {
                regReason.IsReviewed = true;
                regReason.DenialReason = reviewDto.Result ? string.Empty : reviewDto.Reason;
                registrationReasonWriteRepo.Update(regReason);

                var user = userReadRepo.GetById(regReason.UserId);
                user.IsAccountVerified = reviewDto.Result;
                user.IsAccountActive = reviewDto.Result;
                userWriteRepo.Update(user);

                UoW.Commit();

                var mailService = new MailingService(UoW)
                {
                    Body = GenerateReviewMailBody(regReason),
                    Receiver = user.Email,
                    Title = "Registration review"
                };
                mailService.Send();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok(Responses.Ok);
        }

        private string GenerateReviewMailBody(RegistrationReason reason)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Your registration request for account creation has been reviewed!").Append(HtmlWriter.Br());
            stringBuilder.Append(HtmlWriter.Br());
            if (reason.DenialReason == string.Empty)
            {
                stringBuilder.Append("The registration request was accepted!");
            }
            else
            {
                stringBuilder.Append("The registration request was declined...").Append(HtmlWriter.Br());
                stringBuilder.Append(reason.DenialReason);
            }

            return stringBuilder.ToString();
        }
    }
}
