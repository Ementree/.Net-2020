using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNet2020.Data;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using DotNet2020.Controllers;
using System.Reflection;
using DotNet2020.Domain.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using DotNet2020.Domain._3.Controllers;
using DotNet2020.Domain._3.Models.Contexts;

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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
	    #region qwertyRegion
            services.AddDbContext<CompetencesContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DotNet2020.Data")));
            services.AddDbContext<GradesContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DotNet2020.Data")));
            services.AddDbContext<WorkerContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DotNet2020.Data")));
            services.AddDbContext<AttestationContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DotNet2020.Data")));
            #endregion


            var assembly = typeof(DemoController).Assembly;
            var attestationAssembly = typeof(AttestationController).Assembly;

            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.MvcRazorRuntimeCompilationOptions>(
                options =>
                {
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}