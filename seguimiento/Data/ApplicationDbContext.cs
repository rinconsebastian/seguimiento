using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using seguimiento.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace seguimiento.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
    }
}

