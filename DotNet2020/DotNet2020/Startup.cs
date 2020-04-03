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
using DotNet2020.Domain._4.Controllers;
using DotNet2020.Domain.Models;
using Kendo.Mvc.Examples.Models;
using Newtonsoft.Json.Serialization;
using Kendo.Mvc.Examples.Extensions;
using Microsoft.AspNetCore.Http;

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

            #region MAYAK
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // // Add Entity Framework services to the services container.
            services.AddDbContext<SampleEntitiesDataContext>();

            var assembly = typeof(CalendarController).Assembly;

            // Add MVC services to the services container.
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services
                .AddDistributedMemoryCache()
                .AddSession(opts => {
                    opts.Cookie.IsEssential = true;
                });

            // Add Kendo UI services to the services container
            services.AddKendo();

            // Add Demo database services to the services container
            services.AddKendoDemo();
            services.AddSingleton<ReportingConfigurationService>();

            services.AddDbContext<CalendarEntryContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("CalendarEntryContext"), 
                    b => b.MigrationsAssembly("DotNet2020.Data")));
            #endregion

            
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.MvcRazorRuntimeCompilationOptions>(
                options =>
                {
                    options.FileProviders.Add(
                        new EmbeddedFileProvider(assembly));
                });

            //services
            //    .AddMvc()
            //    .AddApplicationPart(assembly)
            //    .AddRazorRuntimeCompilation()
            //    //Mayak
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
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

            #region Mayak
            app.UseSession();
            app.UseCookiePolicy();
            #endregion

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

        public class ReportingConfigurationService
        {
            public IConfiguration Configuration { get; private set; }

            public IWebHostEnvironment Environment { get; private set; }

            public ReportingConfigurationService(IWebHostEnvironment environment)
            {
                this.Environment = environment;

                var configFileName = System.IO.Path.Combine(environment.ContentRootPath, "appsettings.json");
                var config = new ConfigurationBuilder()
                                .AddJsonFile(configFileName, true)
                                .Build();

                this.Configuration = config;
            }
        }
    }
}
