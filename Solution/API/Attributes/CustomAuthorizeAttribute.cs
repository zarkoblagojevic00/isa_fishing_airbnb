using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserType _userType;
        private readonly bool _shouldAuthorizeAll;
        private readonly bool _lastInChain;

        public CustomAuthorizeAttribute(IUnitOfWork uow, bool authorizeAll = true, UserType authorizedUserTypes = UserType.Admin)
        {
            _UnitOfWork = uow;
            _userType = authorizedUserTypes;
            _shouldAuthorizeAll = authorizeAll;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.Request.Cookies[CookieInformation.CookieInformation.Email];
            var userId = context.HttpContext.Request.Cookies[CookieInformation.CookieInformation.UserId];

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(userId))
            {
                if (int.TryParse(userId, out int id))
                {
                    var user = _UnitOfWork.GetRepository<IUserReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.Email == email && x.UserId == id && x.IsAccountActive && x.IsAccountVerified);

                    if (user != null)
                    {
                        if (_shouldAuthorizeAll)
                        {
                            base.OnActionExecuting(context);
                            return;
                        }
                        else
                        {
                            if (user.UserType == _userType)
                            {
                                base.OnActionExecuting(context);
                                return;
                            }
                        }
                    }
                }
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
