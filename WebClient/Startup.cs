using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace WebClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(option =>
                                    {
                                        option.DefaultScheme = "Cookies";
                                        option.DefaultChallengeScheme = "oidc";
                                    })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", option =>
                                     {
                                         option.SignInScheme = "Cookies";

                                         option.Authority = "https://localhost:44348"; //آدرس سرور
                                         option.ClientId = "mvcclient";
                                         option.ClientSecret = "secret";
                                         option.GetClaimsFromUserInfoEndpoint = true;

                                     });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

           
            app.UseMvcWithDefaultRoute();
        }
    }
}
