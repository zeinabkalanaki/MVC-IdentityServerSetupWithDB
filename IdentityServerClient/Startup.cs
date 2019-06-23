using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiClient
{
    public class Startup
    {
        //install IdentityServer4.AccessTokenValidation package
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                            {
                                options.Authority = "https://localhost:44348"; //آدرس سرور
                                options.ApiName = "MyApi"; // همان  AllowedScopes = new []{ "MyApi" } در تعریف کلاینت سرور است
                            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
