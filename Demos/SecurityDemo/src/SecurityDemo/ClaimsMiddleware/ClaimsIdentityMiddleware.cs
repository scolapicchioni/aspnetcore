using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SecurityDemo.ClaimsMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ClaimsIdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsIdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            ((ClaimsIdentity)httpContext.User.Identity).AddClaim(new Claim(ClaimTypes.DateOfBirth, new DateTime(1974, 02, 15).ToString(), ClaimValueTypes.Date, "SIMO_ISSUER"));

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ClaimsIdentityMiddlewareExtensions
    {
        public static IApplicationBuilder UseClaimsIdentityMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsIdentityMiddleware>();
        }
    }
}
