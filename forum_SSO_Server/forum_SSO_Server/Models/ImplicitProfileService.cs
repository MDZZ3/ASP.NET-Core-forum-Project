using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum_SSO_Server.Models
{
    public class ImplicitProfileService : IProfileService
    {
        public  Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
           
                //depending on the scope accessing the user data.
           var claims = context.Subject.Claims.ToList();

            context.IssuedClaims = claims;

            return Task.CompletedTask;
        }

        public  Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.CompletedTask;
        }
    }
}
