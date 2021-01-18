using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using seguimiento.Data;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seguimiento
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
            // Add MVC services to the services container.
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();


            /*services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));*/
            services.AddMvc().AddControllersAsServices();



            services.AddDbContext<ApplicationDbContext>(opt =>
             opt.UseLazyLoadingProxies().UseMySql(
                        // Replace with your connection string.
                        "server=192.168.0.250;user=desarrollo;password=Feserito87@;database=jerico",
                        // Replace with your server version and type.
                        // For common usages, see pull request #1233.
                        new MariaDbServerVersion(new Version(10, 3, 21)), // use MariaDbServerVersion for MariaDB MySqlServerVersion
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    // Everything from this point on is optional but helps with debugging.
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Configuracion.General", policy =>
                                  policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", "1", "2", "3", "4", "5"));

                options.AddPolicy("Configuracion.Responsable", policy =>
                                  policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable", "1"));

                options.AddPolicy("Ejecucion.Editar", policy =>
                                 policy.RequireAssertion(context => context.User.HasClaim(claim =>
                                   (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" && claim.Value == "1") ||
                                   (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" && claim.Value == "1")
                                    )));
                //|| context.User.IsInRole("CEO")));

                options.AddPolicy("Indicador.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar", "1"));

                options.AddPolicy("Periodo.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar", "1"));

                options.AddPolicy("Categoria.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", "1"));
            });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
                endpoints.MapRazorPages();
            });
        }
    }
}
