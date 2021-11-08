using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes
{
    public class CookieMapGlobalAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var requestCookies = context.HttpContext.Request.Cookies;

            foreach (var cookie in requestCookies)
            {
                context.HttpContext.Response.Cookies.Append(cookie.Key, cookie.Value, new CookieOptions()
                {
                    Secure = true
                });
            }

            base.OnResultExecuting(context);
}
    }
}
