using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers.Base
{
    [Controller]
    public abstract class AdvancedController : ControllerBase
    {
        protected readonly IUnitOfWork UoW;
        protected AdvancedController(IUnitOfWork uow)
        {
            UoW = uow;
        }

        protected virtual bool CheckOwnerShip(int serviceId)
        {
            var currentUser = int.Parse(Request.Cookies[CookieInformation.CookieInformation.UserId] ?? string.Empty);
            return new ValidateServiceOwnerService(UoW).ValidateOwnerShip(serviceId, currentUser);
        }
    }
}
