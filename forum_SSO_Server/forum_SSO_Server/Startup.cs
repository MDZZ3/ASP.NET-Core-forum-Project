using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forum.DAL.Entity;
using forum.DAL.Repository;
using forum.Domain.Services;
using forum_SSO_Server.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace forum_SSO_Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddRouting(rout => rout.LowercaseUrls = true);
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Account/Login";

            })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(config.GetApiResource())
                .AddInMemoryClients(config.GetClients())
                .AddInMemoryIdentityResources(config.GetIdentityResources())
                .AddProfileService<ImplicitProfileService>();

            services.AddCors(options =>
            {
                options.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin());
            });

            services.AddSingleton<IUnitOfWork, UnitofWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAccountServices, AccountServices>();
           
            services.AddDbContext<EFContext>((option) => option.UseMySQL(Configuration.GetConnectionString("mysql")));
            
               
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors("cors");

            //app.UseMiddleware<Post302Middleware>();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
