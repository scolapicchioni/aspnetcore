using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurityDemo.Models;

namespace SecurityDemo.AuthorizationHandlers
{
    public class PhotoDeleteAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Photo>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotoDeleteAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    OperationAuthorizationRequirement requirement,
                                                    Photo resource)
        {
            var claimsUser = context.User;
            var userIsAnonymous = claimsUser?.Identity == null || !claimsUser.Identities.Any(i => i.IsAuthenticated);
            if (!userIsAnonymous)
            {
                ApplicationUser user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;
                if (requirement.Name == "Delete" && resource.ApplicationUserId == user.Id)
                    context.Succeed(requirement);
            }
            else {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
