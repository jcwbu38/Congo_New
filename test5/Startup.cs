using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using test5.Models;
using test5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace test5
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
            //var skipSSL = Configuration.GetValue<bool>("LocalTest:skipSSL");
            //// requires using Microsoft.AspNetCore.Mvc;
            //services.Configure<MvcOptions>(options =>
            //{
            //    // Set LocalTest:skipSSL to true to skip SSL requrement in 
            //    // debug mode. This is useful when not using Visual Studio.
            //    if (_hostingEnv.IsDevelopment() && !skipSSL)
            //    {
            //        options.Filters.Add(new RequireHttpsAttribute());
            //    }
            //});


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/User/Login/");
                        options.AccessDeniedPath = new PathString("/User/Forbidden/");
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorsOnly", policy => policy.RequireRole("Administrators"));
                options.AddPolicy("LogisticsOnly", policy => policy.RequireRole("Logistics"));
                options.AddPolicy("SalesOnly", policy => policy.RequireRole("Sales"));
            });

            // Add Inventory DB
            services.AddDbContext<InventoryContext>(options =>
            options.UseSqlite("Data Source=Inventory.db"));

            // Add User DB
            services.AddDbContext<UserContext>(options =>
            options.UseSqlite("Data Source=User.db"));

            // Add Purchase Order DB
            services.AddDbContext<PurchaseOrderContext>(options =>
            options.UseSqlite("Data Source=PurchaseOrder.db"));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

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
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "addToCart",
                    template: "{controller=Home}/{action=Add}/{id}/{qty}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
