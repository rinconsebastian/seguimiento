using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
       //     optionsBuilder.UseMySQL("server=127.0.0.1;database=jerico;user=root;password=1234");
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
        public DbSet<EjecucionCategoria> EjecucionCategoria { get; set; }
        public DbSet<Evaluacion> Evaluacion { get; set; }
        public DbSet<Log> Log { get; set; }

        public DbSet<Nota> Nota { get; set; }

        public DbSet<PermisoRol> PermisoRol { get; set; }
        public DbSet<ValorCampo> ValorCampo { get; set; }
        public DbSet<Policy> Policy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            List<Policy> data = new List<Policy>
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
            modelBuilder.Entity<Policy>().HasData(data);
        }
    }
}

