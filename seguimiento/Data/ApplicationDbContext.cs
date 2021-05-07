using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace seguimiento.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string>
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // {
       //     optionsBuilder.UseMySQL("server=127.0.0.1;database=seguimiento2;user=seguimiento;password=Seguimiento***123");
       // }
        public DbSet<Responsable> Responsable { get; set; }
        public DbSet<Nivel> Nivel { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }

        public DbSet<TipoIndicador> TipoIndicador { get; set; }
       
        public DbSet<Campo> Campo { get; set; }

       
        public DbSet<Periodo> Periodo { get; set; }
       
        public DbSet<Indicador> Indicador { get; set; }
        public DbSet<Ejecucion> Ejecucion { get; set; }
        public DbSet<EjecucionAdjunto> EjecucionAdjunto { get; set; }
        public DbSet<EjecucionCategoria> EjecucionCategoria { get; set; }
        public DbSet<Evaluacion> Evaluacion { get; set; }
        public DbSet<Log> Log { get; set; }

        public DbSet<Nota> Nota { get; set; }

        
        public DbSet<ValorCampo> ValorCampo { get; set; }
        public DbSet<Policy> Policy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Tabla de politicas
            List<Policy> policies = new List<Policy>
            {
                new Policy() { id = 1, nombre = "Ver Configuración general", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General" },
                new Policy() { id = 2, nombre = "Configuración dependencia", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable" },
                new Policy() { id = 3, nombre = "Editar ejecución", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar" },
                new Policy() { id = 4, nombre = "Editar planeación", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar" },
                new Policy() { id = 5, nombre = "Editar indicadores", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar" },
                new Policy() { id = 6, nombre = "Editar periodo", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar" },
                new Policy() { id = 7, nombre = "Editar categorias", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar" },
                new Policy() { id = 8, nombre = "Editar dependencias", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar" },
                new Policy() { id = 9, nombre = "Editar niveles", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar" },
                new Policy() { id = 10, nombre = "Editar campos", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar" },
                new Policy() { id = 11, nombre = "Editar evaluaciones", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar" },
                new Policy() { id = 12, nombre = "Editar roles", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar" },
                new Policy() { id = 13, nombre = "Editar usuarios", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar" },
                new Policy() { id = 14, nombre = "Ver registro actividad", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs" },
                new Policy() { id = 15, nombre = "Editar notas", claim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar" }
                };
            modelBuilder.Entity<Policy>().HasData(policies);

            //Rol administrador
            ApplicationRole rol = new ApplicationRole(){Id = "1", ConcurrencyStamp = "97f6ff5b-6816-44fc-8e6f-bbdedd1223f9", Name = "Administrador", NormalizedName = "ADMINISTRADOR"};
            modelBuilder.Entity<ApplicationRole>().HasData(rol);

            //Permisos rol administrador
            var policiesRol = new List<IdentityRoleClaim<string>>
            {
                new IdentityRoleClaim<string>() { Id =1,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =2,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =3,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =4,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =5,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =6,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =7,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =8,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =9,  RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =10, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =11, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =12, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =13, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =14, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs", ClaimValue = "1"},
                new IdentityRoleClaim<string>() { Id =15, RoleId = "1", ClaimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar", ClaimValue = "1"},
                };
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(policiesRol);

            //Niveles basicos
            var niveles = new List<Nivel>
            {
                new Nivel() { id = 1, numero = 1, color = "000000", nombre = "Plan"},
                new Nivel() { id = 2, numero = 2, color = "000000", nombre = "Categoria"},
                };
            modelBuilder.Entity<Nivel>().HasData(niveles);

            //Entidad por defecto
            Responsable responsable = new Responsable() { Id = 1, Nombre = "Entidad", Editar = true };
            modelBuilder.Entity<Responsable>().HasData(responsable);

            Categoria categoria = new Categoria() { id = 1, idNivel = 1, IdResponsable = 1, nombre = "Principal", unificacion = true, numero = "0.", Ponderador = 1 };
            modelBuilder.Entity<Categoria>().HasData(categoria);

            //Usuario administrador
            ApplicationUser user = new ApplicationUser() { Id = "1", 
                Nombre = "Admin", Apellido = "",
                UserName = "admin@admin.com",NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com", NormalizedEmail = "ADMIN@ADMIN.COM", 
                EmailConfirmed = true, IDDependencia = 1, LockoutEnabled = false,
                PasswordHash = "AQAAAAEAACcQAAAAECPDxHYYnrFlyL6ghv6NFqs7g9ZlRCuHRIgzChzRa5GDZpnwsj563VfwncgzZt+OTw==",
                SecurityStamp = "NNK44MKHKTBOV6DHXJ4BT2Q3SYO3WQC2",
                ConcurrencyStamp = "05622443-5cfd-4389-8879-4523ac4c5aee"
            };
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            //Roles del usuario administrador
            IdentityUserRole<string> userRol = new IdentityUserRole<string>() { RoleId = "1", UserId = "1"};
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRol);

            //Configuración inicial
            Configuracion config  = new Configuracion() { id = 1, anoInicial = 2020, anoFinal = 2021, 
                periodosAnuales = 4, nombrePeriodoAnual = "Trimestre",
                Logo = "/images/SIE.png", contacto = "rinconsebastian@gmail.com", activo = true, 
                Entidad = "Entidad", NombrePlan = "Plan", PonderacionTipo = "PonderacionAbsoluta",
                CalculoNivel = "2", libre = true, EstiloReporte = "",
                colorPrincipal = "#52a3a1", colorTextoHeader = "#ffffff", colorTextoPrincipal = "#00000" };
            modelBuilder.Entity<Configuracion>().HasData(config);

           

            //Evaluaciones globales
            var evaluaciones = new List<Evaluacion>
            {
                new Evaluacion() { Id = 1, Nombre = "Mínimo", Color = "#ff0000", Minimo = 0, Maximo = 60, Contexto = "Global" },
                new Evaluacion() { Id = 2, Nombre = "Aceptable", Color = "#ffff00", Minimo = 60, Maximo = 80, Contexto = "Global" },
                new Evaluacion() { Id = 3, Nombre = "Satisfactorio", Color = "#00ff00", Minimo = 80, Maximo = 100, Contexto = "Global" },
                };
            modelBuilder.Entity<Evaluacion>().HasData(evaluaciones);

            //Tipos indicador
            var tiposIndicador = new List<TipoIndicador>
            {
                new TipoIndicador() { Id = 1, Tipo = "AIncnoacumTmtto", file = "AIncnoacumTmtto", Descripcion = "AIncnoacumTmtto", Enable = true},
                };
            modelBuilder.Entity<TipoIndicador>().HasData(tiposIndicador);

        }
    }
}

