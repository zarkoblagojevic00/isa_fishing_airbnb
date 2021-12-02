using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ConfigurationObjects;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Services;

namespace API.Controllers.Base
{
    [Controller]
    public abstract class AdvancedController : ControllerBase
    {
        protected readonly IUnitOfWork UoW;
        protected readonly FrontDetails Details;

        protected AdvancedController(IUnitOfWork uow, FrontDetails details)
        {
            UoW = uow;
            Details = details;
        }

        protected AdvancedController(IUnitOfWork uow)
        {
            UoW = uow;
        }

        protected virtual bool CheckOwnerShip(int serviceId)
        {
            var currentUser = GetUserIdFromCookie();
            return new ValidateServiceOwnerService(UoW).ValidateOwnerShip(serviceId, currentUser);
        }

        protected virtual int GetUserIdFromCookie()
        {
            var userId = Request.Cookies[CookieInformation.CookieInformation.UserId];

            if (userId == null)
            {
                var cookieFromHeader = Request.Headers["set-cookie"];
                var readCookie = cookieFromHeader.ToArray().First();
                var cookieValues = readCookie.Split("; ");
                userId = cookieValues.First(x => x.StartsWith(CookieInformation.CookieInformation.UserId)).Split("=")[1];
            }

            var currentUser = int.Parse(userId);
            return currentUser;
        }

        protected virtual string GetUserMailFromCookie()
        {
            var email = Request.Cookies[CookieInformation.CookieInformation.Email];

            if (email == null)
            {
                var cookieFromHeader = Request.Headers["set-cookie"];
                var readCookie = cookieFromHeader.ToArray().First();
                var cookieValues = readCookie.Split("; ");
                email = cookieValues.First(x => x.StartsWith(CookieInformation.CookieInformation.Email)).Split("=")[1];
            }

            var currentUser = Request.Cookies[CookieInformation.CookieInformation.Email];
            return currentUser;
        }
    }
}
