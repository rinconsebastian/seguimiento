using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using seguimiento.Data;
using seguimiento.Models;
using seguimiento.Services;
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
                        //"server=127.0.0.1;user=seguimiento;password=Seguimiento***123;database=seguimiento2",
                         "server=192.168.0.250;user=desarrollo;password=Feserito87@;database=seguimientoods",
                        // Replace with your server version and type.
                        // For common usages, see pull request #1233.
                        new MariaDbServerVersion(new Version(10, 4, 10)), // use MariaDbServerVersion for MariaDB MySqlServerVersion
                          //new MySqlServerVersion(new Version(10, 4, 10)),
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    // Everything from this point on is optional but helps with debugging.
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Configuracion.General", policy =>
                                  policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", "1", "2", "3", "4", "5"));
                options.AddPolicy("Configuracion.Logs", policy =>
                             policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs", "1"));

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
                options.AddPolicy("Responsable.Editar", policy =>
                                  policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar", "1"));
                options.AddPolicy("Categoria.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", "1"));
                options.AddPolicy("Nivel.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar", "1"));
                options.AddPolicy("Campo.Editar", policy =>
                                 policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar", "1"));
                options.AddPolicy("Evaluacion.Editar", policy =>
                                policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar", "1"));
                options.AddPolicy("Rol.Editar", policy =>
                                policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar", "1"));
                options.AddPolicy("Usuario.Editar", policy =>
                                policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar", "1"));
                options.AddPolicy("Nota.Editar", policy =>
                               policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar", "1"));


            });


            services.AddControllersWithViews();

            services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"],
                    Configuration["EmailSender:displayName"]
                )
            );
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
                    pattern: "{controller=Dashboard}/{action=Basic}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
