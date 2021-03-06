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

        [HttpGet]
        public IActionResult GetUsersByRole(UserType userType)
        {

            var userInfos = UoW.GetRepository<IUserReadRepository>().GetAll()
                .Where(x => x.UserType == userType);

            List<ExtendedUserInfoDTO> users = new();

            foreach(var user in userInfos)
            {
                var city = UoW.GetRepository<ICityReadRepository>().GetById(user.CityId);
                var country = UoW.GetRepository<ICountryReadRepository>().GetById(city.CountryId);
                var dto = new ExtendedUserInfoDTO()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Surname = user.Surname,
                    Address = user.Address,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    City = city.Name,
                    Country = country.Name,
                    IsAccountVerified = user.IsAccountVerified,
                    IsAccountActive = user.IsAccountActive,
                    UserType = user.UserType,
                };
                users.Add(dto);
            }

            return Ok(users);
        }

        [HttpPost]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
        public IActionResult UpdateBasicInfo(UserInfoDTO userInfo)
        {
            var userId = GetUserIdFromCookie();


            var city = UoW.GetRepository<ICityReadRepository>().GetAll()
                .FirstOrDefault(x => x.Name == userInfo.City);

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

            var services = UoW.GetRepository<IServiceReadRepository>().GetAll()
                .Where(service => service.OwnerId == userId);
            
            var userReservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && x.UserId == userId && !x.IsCanceled);

            var ownerReservationDates = UoW.GetRepository<IReservationReadRepository>()
                .GetAll()
                .Where(x => x.EndDateTime >= DateTime.Now && services.Any(service => service.ServiceId == x.ServiceId) && !x.IsCanceled);


            if (userReservationDates.Any() || ownerReservationDates.Any())
            {
                return BadRequest(Responses.CannotRequestDeletion);
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


        [HttpGet]
        [TypeFilter(typeof(CustomAuthorizeAttribute), Arguments = new object[] { true })]
        public IActionResult GetUsersRequestForDeletion()
        {
            var userId = GetUserIdFromCookie();

            var existingDeletionRequest = UoW.GetRepository<IAccountDeletionRequestReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.UserId == userId);

            return Ok(existingDeletionRequest);
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
