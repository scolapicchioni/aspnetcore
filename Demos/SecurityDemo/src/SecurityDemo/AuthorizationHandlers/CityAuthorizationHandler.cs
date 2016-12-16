using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurityDemo.Models;

namespace SecurityDemo.AuthorizationHandlers
{
    public class CityAuthorizationHandler : AuthorizationHandler<CityRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CityAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CityRequirement requirement)
        {
            var claimsUser = context.User;
            var userIsAnonymous = claimsUser?.Identity == null || !claimsUser.Identities.Any(i => i.IsAuthenticated);
            if (!userIsAnonymous){
                ApplicationUser user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;
                if (user.City == requirement.City)
                    context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

   
}
