using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerSetup.Infrastructure
{
    public static class IdentityData
    {
        public static List<ApiResource> GetApiResources()
        {
            return
             new List<ApiResource> {
                new ApiResource("MyApi","My Api display")
            };
        }

        public static List<Client> GetClients()
        {
            return
             new List<Client>{
                new Client() // for ApiClient 
                {
                    /* for get "access token" : ClientId,ClientSecrets,AllowedGrantTypes is enough and
                     * make post requet to https://localhost:44348/connect/token */

                    ClientId = "postmanclient",
                    ClientSecrets = new []{ new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials, //هر کلاینت می تواند فلو مخصوص خود را داشته باشد
                    AllowedScopes = new []{ "MyApi" }
                },
                new Client()
                {
                    ClientId = "mvcclient",
                    ClientName = "Mvc web Client",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris = { "https://localhost:44319/signin-oidc" },//آدرس سرور
                    PostLogoutRedirectUris = { "https://localhost:44319/signout-callback-oidc" },//آدرس سرور

                    AllowedScopes = new List<string> //کلاینت چه دیتایی از کاربر دریافت کند
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                       // StandardScopes.Email //این کلاینت به ایمیل دسترسی ندارد
                    }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return
             new List<TestUser>{
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "Reyhaneh",
                    Password = "p@ssWord"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            //تمام AllowedScopes باید اینجا تعریف شود بعد هر کلاینتی به یک زیر مجموعه از این دسترسی دارد
            return new List<IdentityResource> 
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                     new IdentityResources.Email(),

                };
        }
    }

}
