using System;
using System.IO;
using System.Threading.Tasks;
using DotNet2020.Data;
using DotNet2020.Domain._3.Controllers;
using DotNet2020.Domain._4.Controllers;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._5.Controllers;
using DotNet2020.Domain._6.Controllers;
using DotNet2020.Domain.Core.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DotNet2020
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppIdentityUser, AppIdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                 .AddDefaultUI();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddKendo();

            var attestationAssembly = typeof(AttestationController).Assembly;
            var domain4Assembly = typeof(CalendarController).Assembly;
            var assembly = typeof(DemoController).Assembly;
            var domain6Assembly = typeof(PlanController).Assembly;
            var domain5Assembly = typeof(ReportController).Assembly;

            services.Configure<MvcRazorRuntimeCompilationOptions>(
                options =>
                {
                    options.FileProviders.Add(
                        new EmbeddedFileProvider(domain4Assembly));
                    options.FileProviders.Add(
                        new EmbeddedFileProvider(domain6Assembly));
                    options.FileProviders.Add(
                        new EmbeddedFileProvider(domain5Assembly));
                    options.FileProviders.Add(
                        new EmbeddedFileProvider(assembly));

                    options.FileProviders.Add(
                        new EmbeddedFileProvider(attestationAssembly));
                });

            services
                .AddMvc()
                .AddApplicationPart(assembly)
                .AddApplicationPart(attestationAssembly)
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (!env.IsDevelopment()) app.UseHttpsRedirection();


            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath + ".Domain.6", "wwwroot6")),
                RequestPath = "/wwwroot6"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRolesAndAdminIfNotExists(serviceProvider).Wait();
        }

        public async Task CreateRolesAndAdminIfNotExists(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
            string[] roleNames = { "admin" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new AppIdentityRole { Name = roleName });
                }
            }

            var admin = new AppIdentityUser()
            {
                Email = Configuration["AdminLogin"],
                UserName = Configuration["AdminLogin"]
            };

            string adminPassword = Configuration["AdminPassword"];

            var createdAdmin = await UserManager.FindByNameAsync(Configuration["AdminLogin"]);

            if (createdAdmin == null)
            {
                var createPowerUser = await UserManager.CreateAsync(admin, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}