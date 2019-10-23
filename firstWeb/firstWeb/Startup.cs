using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using firstWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using firstWeb.Domain.Services.forum;
using firstWeb.Domain.Services.forum_Category;
using firstWeb.Domain.Services.comment;
using firstWeb.Domain.Services.accountService;
using firstWeb.Domain.Even;
using firstWeb.Domain.Options.JsonOptions;
using firstWeb.Domain.Services.contern;
using firstWeb.Middleware;
using firstWeb.Domain.Services.notice;

namespace firstWeb
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
            services.AddHttpClient("client1", (hc) =>
            {
                hc.BaseAddress = new Uri(Configuration.GetConnectionString("AuthenticationServer"));
            });

            services.AddSession();
            services.AddMvc().AddJsonOptions(options=>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                options.SerializerSettings.ContractResolver = new NullToEmptyResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            
            


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                
            }).AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
             {
                 options.Authority = Configuration.GetSection("Authentication:AuthenticationServer").Value;
                 options.RequireHttpsMetadata = false;
                 options.ClientId =Configuration.GetSection("Authentication:ClientId").Value;
                 options.ClientSecret =Configuration.GetSection("Authentication:ClientSecret").Value;

             });

            services.AddHttpClient("forum_Server", hc =>
             {
                 hc.BaseAddress = new Uri("http://localhost:5000");
             });

            //注入DbContext
            services.AddDbContextPool<forumDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConntection")));

            services.AddScoped(typeof(IRepository<>), typeof(forumRepository<>));

            services.AddScoped<IForumService, ForumService>();

            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IforumCategoryService, forumCategoryService>();

            services.AddScoped<IConternService, ConternService>();

            services.AddScoped<INoticeService, NoticeService>();

            //订阅发布
            services.AddScoped<AddReplyCountHandler>();
            services.AddScoped<EvenHandlerContainer>();
            services.AddScoped<AddCommentCountHandler>();
            services.AddScoped<AddForumCountHandler>();
            services.AddScoped<Comment_NoticeHandler>();
            services.AddScoped<Reply_NoticeHandler>();
            services.AddScoped<Concern_NoticeHandler>();

            EvenHandlerContainer.Subscribe<ReplySumbitEven, AddReplyCountHandler>("AddReply");
            EvenHandlerContainer.Subscribe<ReplySumbitEven, Reply_NoticeHandler>("AddReply");
            EvenHandlerContainer.Subscribe<CommentSubmitEven, AddCommentCountHandler>("AddComment");
            EvenHandlerContainer.Subscribe<CommentSubmitEven, Comment_NoticeHandler>("AddComment");
            EvenHandlerContainer.Subscribe<CreateForumSumbitEven, AddForumCountHandler>("CreateForum");
            EvenHandlerContainer.Subscribe<ConcernSumbitEven, Concern_NoticeHandler>("CreateConcern");
            
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //全局设置如果响应是404，则跳转到404页面
           // app.UseMiddleware<code404Middleware>();
          

            app.UseAuthentication();
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}");
            });
            
        }
    }
}
