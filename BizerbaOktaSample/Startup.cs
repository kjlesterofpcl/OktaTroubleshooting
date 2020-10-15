using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BizerbaOktaSample.Utilities;
using BizerbaOktaSample.Utilities.Okta;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Okta.AspNetCore;

namespace BizerbaOktaSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.Configure<OktaSettings>(this.Configuration.GetSection("Okta"));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IAuthorizationGateway, OktaGateway>();
            
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
                     {
                         options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                         options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                     })
                    .AddCookie()
                    .AddOktaMvc(new OktaMvcOptions()
                     {
                         OktaDomain = this.Configuration["Okta:OktaDomain"],
                         ClientId = this.Configuration["Okta:ClientId"],
                         ClientSecret = this.Configuration["Okta:ClientSecret"],
                         PostLogoutRedirectUri = this.Configuration["Okta:PostLogoutRedirectUri"]
                     });
            
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        public IConfiguration Configuration
        {
            get;
            private set;
        }
    }
}