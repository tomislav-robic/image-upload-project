using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Image_upload_project.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Image_upload_project.Data;
using Image_upload_project.Models.Image;
using Image_upload_project.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Image_upload_project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            RegisterCustomDependencies(services);
            
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        private void RegisterCustomDependencies(IServiceCollection services)
        {
            //settings
            var databaseSettings = new DatabaseSettings(Configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(databaseSettings);
            var imageStorageSettings = new ImageStorageSettings();
            imageStorageSettings.ImageRepositoryPath = Configuration.GetValue<string>("ImageRepositoryPath");
            services.AddSingleton(imageStorageSettings);
            //repositories
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            //other
            services.AddSingleton<ImageBuilderFactory>();
            services.AddScoped<AuthorizationService>();
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
            app.UseHttpsRedirection();
            //give access to everything in wwwroot
            app.UseStaticFiles();
            //map our image storage location to a relative route to enable displaying them on the site
            var imageRepositoryPath = Configuration.GetValue<string>("ImageRepositoryPath");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(imageRepositoryPath),
                RequestPath = "/userImages"
            });

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
