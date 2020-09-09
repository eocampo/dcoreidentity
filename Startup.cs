using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Identity.EntityFrameworkCore;
using dcoreidentity.Models;
using dcoreidentity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace dcoreidentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            var identityConnectionString = Configuration["ConnectionStrings:IdentityConnection"];
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(identityConnectionString
                , sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentityCore<CustomUser>(options => { });
            // services.AddScoped<IUserStore<CustomUser>, MemoryUserStore>();
            services.AddScoped<IUserStore<IdentityUser>,
                 UserOnlyStore<IdentityUser, IdentityDbContext>>();
            services.AddAuthentication("Cookies") //CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("Cookies", options => options.LoginPath = "/Account/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
