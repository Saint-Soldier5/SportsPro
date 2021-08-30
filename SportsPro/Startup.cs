using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SportsPro.Models;
using System;
using SportsPro.DataLayer.Repositories;

namespace SportsPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //configure app to use session state
            //services must be called before AddControllersWithViews()
            services.AddMemoryCache();
            //changed session timeout to 5 min; see pg 325 of Murach textbook
            services.AddSession(options=>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60 * 5);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
           

            services.AddControllersWithViews().AddNewtonsoftJson(); // add MVC services, added NewtonsoftJson library

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SportsPro")));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });

            //Identity service 
            services.AddIdentity<User, IdentityRole>
                (options => {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<SportsProContext>()
                .AddDefaultTokenProviders();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ISportsProUnitOfWork, SportsProUnitOfWork>();
            //add other services here
        }

        // Use this method to configure the HTTP request pipeline.
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

            app.UseRouting();  // mark where routing decisions are made

            //configure middleware that runs after routing decisions have been made
            //Configure app to use authentication and authorization (p.s. Order Matters!)
            app.UseAuthentication();
            app.UseAuthorization();

            //configure app to use session state
            //services must be called before UseEndpoints()

            app.UseSession();
            app.UseEndpoints(endpoints =>    // map the endpoints
            {
                //specific route - 1 required segment
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=User}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "tech",
                    areaName: "Technician",
                    pattern: "Technician/{controller=User}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "Index",
                    pattern: "{controller}/{action=Index}/{id?}");

                //least specific route - 0 required segments
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SportsProContext.CreateAdminUser(app.ApplicationServices).Wait();

            // configure other middleware here
        }
    }
}