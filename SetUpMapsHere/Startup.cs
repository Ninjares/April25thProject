using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPDbContext;
using ASPDbContext.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SetUpMapsHere.Hub;
using SetUpMapsHere.Services;

namespace SetUpMapsHere
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
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddDbContext<TransportDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddRoles<ApplicationRole>().AddEntityFrameworkStores<TransportDbContext>().AddDefaultUI().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = $"/User/Login";
                options.LogoutPath = $"/User/Logout";
            });
            services.AddControllersWithViews();
            services.AddTransient<IOSMService, OSMService>();
            services.AddTransient<IAdminService, AdminServices>();
            services.AddTransient<IDriverService, DriverService>();
            services.AddSingleton<ILocationService>(new LocationService());
            services.AddAuthentication();
            services.AddRazorPages();
            services.AddSignalR();

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
                endpoints.MapHub<GpsHub>("/gps");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
