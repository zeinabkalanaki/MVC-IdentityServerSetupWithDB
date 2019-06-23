using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerSetup.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace IdentityServerSetup
{
    public class Startup
    {
        //OAuth and openId is a protocol
        //Install IdentityServer4 package
        //Go To https://localhost:44362/.well-known/openid-configuration to see Identity Server
        //For add Ui go to https://github.com/identityserver/identityserver4.QuickStart.ui and run iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/master/getmaster.ps1'))

        //install IdentityServer4.EntityFramework
        //install Microsoft.EntityFrameworkCore.SqlServer
        //install Microsoft.EntityFrameworkCore.Tools

        //add-migration initConfigureDbContext -context ConfigurationDbContext
        //add-migration initPersistedGrantDbContext -context PersistedGrantDbContext
        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=IdentityServer4Db;Data Source=.";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()

                    .AddDeveloperSigningCredential()
                    //یک پرایویت کی باید در سرور باشد که بتواند اطلاعات را با آن ساین کند و کلایت ها هم باید پابلیک کی داشته باشند تا بتوانند دیتای ساین شده را باز کنند
                    //کد بالا جهت ایجاد کی های مورد نیاز در محیط توسعه می باشد

                    .AddResourceOwnerValidator<ResourceOwnerValidator>() //به جای .AddTestUsers(IdentityData.GetTestUsers())

                    .AddConfigurationStore(o =>
                      {
                          o.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                      })
                    .AddOperationalStore(o =>
                      {
                          o.ConfigureDbContext = builder =>
                               builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                          o.EnableTokenCleanup = true;
                          o.TokenCleanupInterval = 30;
                      });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeDatabases(app);

            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabases(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityData.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityData.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityData.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
