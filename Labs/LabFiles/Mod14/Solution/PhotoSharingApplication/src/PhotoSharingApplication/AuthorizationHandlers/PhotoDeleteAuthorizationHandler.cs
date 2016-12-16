using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using PhotoSharingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.AuthorizationHandlers
{
    public class PhotoDeleteAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Photo>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotoDeleteAuthorizationHandler(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    OperationAuthorizationRequirement requirement,
                                                    Photo photo) {
            var claimsUser = context.User;
            var userIsAnonymous = claimsUser?.Identity == null || !claimsUser.Identities.Any(i => i.IsAuthenticated);
            if (!userIsAnonymous) {
                ApplicationUser user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;
                if (requirement.Name == "Delete")
                {
                    if (photo.ApplicationUserId == user.Id)
                        context.Succeed(requirement);
                    else
                        context.Fail();
                }
            }
            else
                context.Fail();
            
            return Task.CompletedTask;
        }
    }
}
