using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;
using API.ConfigurationObjects;
using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using NHibernate.Proxy;
using Services.Constants;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GeneralUserController : AdvancedController
    {
        public GeneralUserController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
        public IActionResult GetBasicInfo()
        {
            var userId = GetUserIdFromCookie();

            var userInfo = UoW.GetRepository<IUserReadRepository>().GetById(userId);
            var city = UoW.GetRepository<ICityReadRepository>().GetById(userInfo.CityId);
            var country = UoW.GetRepository<ICountryReadRepository>().GetById(city.CountryId);
            
            var dto = new UserInfoDTO()
            {
                Name = userInfo.Name,
                Surname = userInfo.Surname,
                Address = userInfo.Address,
                Phone = userInfo.PhoneNumber,
                Email = userInfo.Email,
                City = city.Name,
                Country = country.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
        public IActionResult UpdateBasicInfo(UserInfoDTO userInfo)
        {
            var userId = GetUserIdFromCookie();

            var country = UoW.GetRepository<ICountryReadRepository>().GetAll()
                .FirstOrDefault(x => x.Name == userInfo.Country);

            if (country == null)
            {
                return BadRequest(Responses.NotFound);
            }

            var city = UoW.GetRepository<ICityReadRepository>().GetAll()
                .FirstOrDefault(x => x.Name == userInfo.City && x.CountryId == country.CountryId);

            if (city == null)
            {
                return BadRequest(Responses.NotFound);
            }

            var user = UoW.GetRepository<IUserReadRepository>().GetById(userId);

            try
            {
                UoW.BeginTransaction();

                MapBasicInfo(userInfo, user, city);
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

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
        public IActionResult RequestAccountDeletion(AccountDeletionRequestDTO request)
        {
            var userId = GetUserIdFromCookie();

            var existingDeletionRequest = UoW.GetRepository<IAccountDeletionRequestReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.UserId == userId);

            if (existingDeletionRequest != null)
            {
                return BadRequest(Responses.AccountDeletionRequestSubmitted);
            }

            try
            {
                UoW.BeginTransaction();

                var newDeletionRequest = CreateNewDeletionRequest(request, userId);
                UoW.GetRepository<IAccountDeletionRequestWriteRepository>().Add(newDeletionRequest);

                UoW.Commit();
            }
            catch (Exception e)
            {
                UoW.Rollback();
                return Problem(e.Message);
            }

            return Ok(Responses.Ok);
        }

        [HttpPut]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
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


        private void MapBasicInfo(UserInfoDTO newInfo, User user, City city)
        {
            user.Name = newInfo.Name;
            user.Surname = newInfo.Surname;
            user.Address = newInfo.Address;
            user.PhoneNumber = newInfo.Phone;
            user.CityId = city.CityId;
        }

        private AccountDeletionRequest CreateNewDeletionRequest(AccountDeletionRequestDTO request, int userId)
        {
            return new AccountDeletionRequest()
            {
                Reason = request.Reason,
                RequestedDateTime = DateTime.Now,
                UserId = userId
            };
        }
    }
}
