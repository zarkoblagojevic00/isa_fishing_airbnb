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
    public class CustomAuthorizeServiceOwnerAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomAuthorizeServiceOwnerAttribute(IUnitOfWork uow)
        {
            _UnitOfWork = uow;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.Request.Cookies[CookieInformation.CookieInformation.Email];
            var userId = context.HttpContext.Request.Cookies[CookieInformation.CookieInformation.UserId];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId))
            {
                if (context.HttpContext.Request.Headers.TryGetValue("set-cookie", out var cookieInfo))
                {
                    var values = cookieInfo.ToArray();
                    var readCookie = values.FirstOrDefault();

                    if (!string.IsNullOrEmpty(readCookie))
                    {
                        values = readCookie.Split("; ");
                        foreach (var value in values)
                        {
                            if (value.StartsWith(CookieInformation.CookieInformation.Email))
                            {
                                email = value.Split("=")[1];
                            }
                            else if (value.StartsWith(CookieInformation.CookieInformation.UserId))
                            {
                                userId = value.Split("=")[1];
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(userId))
            {
                if (int.TryParse(userId, out int id))
                {
                    var user = _UnitOfWork.GetRepository<IUserReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.Email == email && x.UserId == id && x.IsAccountActive && x.IsAccountVerified &&
                                             x.UserType != UserType.Admin && x.UserType != UserType.Registered);

                    if (user != null)
                    {
                        base.OnActionExecuting(context);
                        return;
                    }
                }
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
