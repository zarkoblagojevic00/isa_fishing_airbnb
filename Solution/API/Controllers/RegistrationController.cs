using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using MimeKit.Text;
using Services;
using Services.Constants;
using Services.Validators;
using HtmlWriter = Services.HtmlWriter.HtmlWriter;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public RegistrationController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public IActionResult RegisterServiceOwner(ServiceOwnerRegistrationDTO registrationForm)
        {
            var emailValidationService = new EmailValidationService(_uow);
            var serviceResponse = emailValidationService.CheckEmailDuplication(registrationForm.Email);
            if (serviceResponse != Responses.Ok)
            {
                return BadRequest(serviceResponse);
            }

            var writeUserRepo = _uow.GetRepository<IUserWriteRepository>();
            var writeReasonRepo = _uow.GetRepository<IRegistrationReasonWriteRepository>();
            var writeUserVerificationRepo = _uow.GetRepository<IUserVerificationKeyWriteRepository>();

            _uow.BeginTransaction();
            
            var newUser = ExtractUser(registrationForm);
            writeUserRepo.Add(newUser);

            var registrationReason = ExtractRegistrationReason(registrationForm);
            registrationReason.UserId = newUser.UserId;
            writeReasonRepo.Add(registrationReason);

            var userRegistrationKey = new UserVerificationKey()
            {
                UserId = newUser.UserId,
                VerificationKey = Guid.NewGuid(),
                IsUsed = false
            };

            writeUserVerificationRepo.Add(userRegistrationKey);

            _uow.Commit();

            var mailService = new MailingService(_uow)
            {
                Body = GenerateMailForConfirmation(newUser.UserId, userRegistrationKey.VerificationKey.ToString()),
                Receiver = newUser.Email,
                Title = "Account verification"
            };
            mailService.Send();

            return Ok(Responses.Ok);
        }

        [HttpGet]
        public IActionResult ConfirmIdentity(int userId, Guid guid)
        {
            var verificationKeyReadRepo = _uow.GetRepository<IUserVerificationKeyReadRepository>();
            var key = verificationKeyReadRepo.GetAll()
                .FirstOrDefault(x => x.UserId == userId && x.VerificationKey == guid);

            if (key == null)
            {
                return BadRequest(Responses.VerificationKeyNotFound);
            }

            if (key.IsUsed)
            {
                return BadRequest(Responses.VerificationKeyUsed);
            }

            var user = _uow.GetRepository<IUserReadRepository>().GetById(userId);

            _uow.BeginTransaction();

            key.IsUsed = true;
            _uow.GetRepository<IUserVerificationKeyWriteRepository>().Update(key);

            user.IsAccountVerified = true;
            _uow.GetRepository<IUserWriteRepository>().Update(user);

            _uow.Commit();

            return Ok(Responses.Ok);
        }

        private User ExtractUser(ServiceOwnerRegistrationDTO registrationForm)
        {
            return new User()
            {
                UserType = registrationForm.UserType,
                Name = registrationForm.Name,
                Surname = registrationForm.Surname,
                Password = registrationForm.Password,
                Address = registrationForm.Address,
                CityId = registrationForm.CityId,
                PhoneNumber = registrationForm.PhoneNumber,
                Email = registrationForm.Email,
                IsAccountActive = false,
                IsAccountVerified = false
            };
        }

        private RegistrationReason ExtractRegistrationReason(ServiceOwnerRegistrationDTO registrationForm)
        {
            return new RegistrationReason()
            {
                Reason = registrationForm.Reason,
                IsReviewed = false
            };
        }

        private string GenerateMailForConfirmation(int id, string guid)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("In order to confirm your identity you need to click on the link below").Append(HtmlWriter.Br());
            stringBuilder.Append(HtmlWriter.Br());
            stringBuilder.Append(HtmlWriter.A(Url.Action("ConfirmIdentity", "Registration",new {userId = id, guid}, Request.Scheme),"link"));

            return stringBuilder.ToString();
        }
    }
}
