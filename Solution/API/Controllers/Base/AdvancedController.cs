using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ConfigurationObjects;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
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
            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            return new ValidateServiceOwnerService(UoW).ValidateOwnerShip(serviceId, currentUser);
        }

        protected virtual int GetUserIdFromCookie()
        {
            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            return currentUser;
        }

        protected virtual string GetUserMailFromCookie()
        {
            var currentUser = Request.Cookies[CookieInformation.CookieInformation.Email];
            return currentUser;
        }
    }
}
