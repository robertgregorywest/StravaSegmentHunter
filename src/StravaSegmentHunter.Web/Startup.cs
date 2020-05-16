using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using StravaSegmentHunter.OAuth;

namespace StravaSegmentHunter.Web
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
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = StravaAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddStrava(options =>
            {
                options.ClientId = Configuration.GetValue<string>("Strava:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("Strava:ClientSecret");
                options.Scope.Add("activity:read_all");
                options.Scope.Add("profile:read_all");
                options.SaveTokens = true;
            });
            
            // adds user access token management
            services.AddAccessTokenManagement(options =>
                {
                    options.User.Scheme = StravaAuthenticationDefaults.AuthenticationScheme;
                })
            .ConfigureBackchannelHttpClient(options =>
            {
                options.BaseAddress = new Uri(StravaAuthenticationDefaults.TokenEndpoint);
            })
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            }));

            // registers a typed HTTP client with token management support
            services.AddHttpClient<StravaClient>(client =>
            {
                client.BaseAddress = new Uri(StravaAuthenticationDefaults.BaseAddress);
            })
            .AddUserAccessTokenHandler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}