using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace forum_SSO_Server
{
    public class config
    {
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api",new List<string>{ClaimTypes.Role })
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="forum",

                    ClientName="forum_SSO",

                    AllowedGrantTypes=GrantTypes.Implicit,

                    RequireConsent=false,

                    //允许令牌和授权码返回的地址
                    RedirectUris={"http://localhost:5001/signin-oidc"},
                    //指定允许注销后跳转的地址
                    PostLogoutRedirectUris={"http://localhost:5001/signout-callback-oidc"},

                    ClientSecrets={ new Secret("Secret".Sha256()) },
                    AllowedScopes=new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                        
                    }
                }
            };
        }

    }
}
