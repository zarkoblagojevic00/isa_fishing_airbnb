﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.ConfigurationObjects;
using API.Controllers.Base;
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
    [Route("api/[controller]/[action]")]
    public class RegistrationController : AdvancedController
    {
        public RegistrationController(IUnitOfWork uow, FrontDetails details) : base(uow, details) {}

        [HttpPost]
        public IActionResult RegisterUser(UserRegistrationDTO userRegistrationData)
        {
            var emailValidationService = new EmailValidationService(UoW);
            var serviceResponse = emailValidationService.CheckEmailDuplication(userRegistrationData.Email);
            if (serviceResponse != Responses.Ok)
            {
                return BadRequest(serviceResponse);
            }

            var writeUserRepo = UoW.GetRepository<IUserWriteRepository>();
            var writeUserVerificationRepo = UoW.GetRepository<IUserVerificationKeyWriteRepository>();

            UoW.BeginTransaction();

            var newUser = ExtractServiceUser(userRegistrationData);
            writeUserRepo.Add(newUser);

            var userRegistrationKey = new UserVerificationKey()
            {
                UserId = newUser.UserId,
                VerificationKey = Guid.NewGuid(),
                IsUsed = false
            };

            writeUserVerificationRepo.Add(userRegistrationKey);

            UoW.Commit();

            var mailService = new MailingService(UoW)
            {
                Body = GenerateMailForConfirmation(newUser.UserId, userRegistrationKey.VerificationKey.ToString()),
                Receiver = newUser.Email,
                Title = "Account verification"
            };
            mailService.Send();

            return Ok(Responses.Ok);
        }


        private User ExtractServiceUser(UserRegistrationDTO userRegistrationData)
        {
            return new User()
            {
                UserType = UserType.Registered,
                Name = userRegistrationData.Name,
                Surname = userRegistrationData.Surname,
                Password = userRegistrationData.Password,
                Address = userRegistrationData.Address,
                CityId = userRegistrationData.CityId,
                PhoneNumber = userRegistrationData.PhoneNumber,
                Email = userRegistrationData.Email,
                IsAccountActive = true,
                IsAccountVerified = false
            };
        }

        [HttpPost]
        public IActionResult RegisterServiceOwner(ServiceOwnerRegistrationDTO userRegistrationData)
        {
            var emailValidationService = new EmailValidationService(UoW);
            var serviceResponse = emailValidationService.CheckEmailDuplication(userRegistrationData.Email);
            if (serviceResponse != Responses.Ok)
            {
                return BadRequest(serviceResponse);
            }

            var writeUserRepo = UoW.GetRepository<IUserWriteRepository>();
            var writeReasonRepo = UoW.GetRepository<IRegistrationReasonWriteRepository>();
            var writeUserVerificationRepo = UoW.GetRepository<IUserVerificationKeyWriteRepository>();

            UoW.BeginTransaction();
            
            var newUser = ExtractServiceOwner(userRegistrationData);
            writeUserRepo.Add(newUser);

            var registrationReason = ExtractRegistrationReason(userRegistrationData);
            registrationReason.UserId = newUser.UserId;
            writeReasonRepo.Add(registrationReason);

            var userRegistrationKey = new UserVerificationKey()
            {
                UserId = newUser.UserId,
                VerificationKey = Guid.NewGuid(),
                IsUsed = false
            };

            writeUserVerificationRepo.Add(userRegistrationKey);

            UoW.Commit();

            var mailService = new MailingService(UoW)
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
            var verificationKeyReadRepo = UoW.GetRepository<IUserVerificationKeyReadRepository>();
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

            var user = UoW.GetRepository<IUserReadRepository>().GetById(userId);

            UoW.BeginTransaction();

            key.IsUsed = true;
            UoW.GetRepository<IUserVerificationKeyWriteRepository>().Update(key);

            user.IsAccountVerified = true;
            UoW.GetRepository<IUserWriteRepository>().Update(user);

            UoW.Commit();

            return Redirect(Details.BaseUrl + "login");
        }

        private User ExtractServiceOwner(ServiceOwnerRegistrationDTO userRegistrationData)
        {
            return new User()
            {
                UserType = userRegistrationData.UserType,
                Name = userRegistrationData.Name,
                Surname = userRegistrationData.Surname,
                Password = userRegistrationData.Password,
                Address = userRegistrationData.Address,
                CityId = userRegistrationData.CityId,
                PhoneNumber = userRegistrationData.PhoneNumber,
                Email = userRegistrationData.Email,
                IsAccountActive = false,
                IsAccountVerified = false
            };
        }

        private RegistrationReason ExtractRegistrationReason(ServiceOwnerRegistrationDTO userRegistrationData)
        {
            return new RegistrationReason()
            {
                Reason = userRegistrationData.Reason,
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
