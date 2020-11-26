using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TrackerWebAPI
{
    public class Startup
    {

        readonly string MyPolicy = "_myPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/Account/login";
                })
                .AddGoogle(googleOptions => {
                    googleOptions.ClientId = "829498266017-32fjff71n24bn68rvgg0eabhigk4mpnl.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "6ur-1uiMx08XDlywlcBMGsOw";
                })
                .AddFacebook(facebookOptions => {
                    facebookOptions.ClientId = "433171077848490";
                    facebookOptions.ClientSecret = "7a3ea6aaece73e8d2f2762d553233113";
                });
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name : MyPolicy,
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:5500")
                               .AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyPolicy);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
