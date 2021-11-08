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
        private readonly IUnitOfWork UnitOfWork;
        private readonly UserType userType;
        private readonly bool shouldAuthorizeAll;

        public CustomAuthorizeAttribute(IUnitOfWork uow, bool authorizeAll = true, UserType authorizedUserTypes = UserType.Admin)
        {
            UnitOfWork = uow;
            userType = authorizedUserTypes;
            shouldAuthorizeAll = authorizeAll;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.Request.Cookies["email"];
            var userId = context.HttpContext.Request.Cookies["userId"];

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(userId))
            {
                if (int.TryParse(userId, out int id))
                {
                    var user = UnitOfWork.GetRepository<IUserReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.Email == email && x.UserId == id && x.IsAccountActive);

                    if (user != null)
                    {
                        if (shouldAuthorizeAll)
                        {
                            base.OnActionExecuting(context);
                            return;
                        }
                        else
                        {
                            if (user.UserType == userType)
                            {
                                base.OnActionExecuting(context);
                                return;
                            }
                        }
                    }
                }
            }

            context.Result = new BadRequestResult();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
