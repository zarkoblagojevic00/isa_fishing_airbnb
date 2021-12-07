using API.Controllers.Base;
using API.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : AdvancedController
    {
        public LoginController(IUnitOfWork uow) : base(uow)
        { }

        [HttpPost]
        public IActionResult LoginUser(UserCredentialsDTO userCredentials)
        {
            var readUserRepo = UoW.GetRepository<IUserReadRepository>();
            var userLoggingIn = readUserRepo.GetAll().FirstOrDefault<User>(u => u.Email.Equals(userCredentials.Email));
            if (!AreCredentialsValid(userCredentials, userLoggingIn))
            {
                return BadRequest(Responses.BadCredentials);
            }
            if (!IsAccountValid(userLoggingIn))
            {
                return BadRequest(Responses.UserAccountNotVerified);
            }

            return Ok(new UserLoginResponseDTO(userLoggingIn));
        }

        private static bool AreCredentialsValid(UserCredentialsDTO userCredentials, User userLoggingIn)
        {
            return userLoggingIn != null && userLoggingIn.Password.Equals(userCredentials.Password);
        }

        private static bool IsAccountValid(User userLoggingIn)
        {
            return userLoggingIn.IsAccountActive && userLoggingIn.IsAccountVerified;
        }
    }
}
