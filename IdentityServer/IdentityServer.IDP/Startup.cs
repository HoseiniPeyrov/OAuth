using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.IDP.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.IDP
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityServer()
            .AddDeveloperSigningCredential() // تنظیمات کلید امضای دیجیتال توکن‌ها را انجام می‌دهد
            .AddTestUsers(Config.GetUsers())  // لیست کاربران و اطلاعات آن‌ها
            .AddInMemoryIdentityResources(Config.GetIdentityResources())  // لیست منابع و Scopes
            .AddInMemoryClients(Config.GetClients());           // لیست کلاینت‌ها 


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseIdentityServer();  // افزودن میان افزار  به برنامه

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
