﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using seguimiento.Data;

namespace seguimiento.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210123212120_piliticas")]
    partial class piliticas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("seguimiento.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("seguimiento.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Apellido")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("IDDependencia")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("seguimiento.Models.Campo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Activado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("NivelPadreid")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("TipoIndicadorPadreId")
                        .HasColumnType("int");

                    b.Property<bool>("TodaCategoria")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("TodoIndicador")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("NivelPadreid");

                    b.HasIndex("TipoIndicadorPadreId");

                    b.ToTable("Campo");
                });

            modelBuilder.Entity("seguimiento.Models.Categoria", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IdResponsable")
                        .HasColumnType("int");

                    b.Property<decimal>("Ponderador")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("idCategoria")
                        .HasColumnType("int");

                    b.Property<int>("idNivel")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nota")
                        .HasColumnType("longtext");

                    b.Property<string>("numero")
                        .HasColumnType("longtext");

                    b.Property<string>("objetivo")
                        .HasColumnType("longtext");

                    b.Property<string>("texto")
                        .HasColumnType("longtext");

                    b.Property<bool>("unificacion")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("id");

                    b.HasIndex("IdResponsable");

                    b.HasIndex("idCategoria");

                    b.HasIndex("idNivel");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("seguimiento.Models.Configuracion", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CalculoNivel")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Entidad")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ImgBackgroud")
                        .HasColumnType("longtext");

                    b.Property<string>("ImgHeader")
                        .HasColumnType("longtext");

                    b.Property<string>("Logo")
                        .HasColumnType("longtext");

                    b.Property<string>("NombrePlan")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PonderacionTipo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("anoFinal")
                        .HasColumnType("int");

                    b.Property<int>("anoInicial")
                        .HasColumnType("int");

                    b.Property<string>("contacto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("libre")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("nombrePeriodoAnual")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("periodosAnuales")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Configuracion");
                });

            modelBuilder.Entity("seguimiento.Models.Ejecucion", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nota")
                        .HasColumnType("longtext");

                    b.Property<string>("adjunto")
                        .HasColumnType("longtext");

                    b.Property<bool>("cargado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ejecutado")
                        .HasColumnType("longtext");

                    b.Property<int>("idindicador")
                        .HasColumnType("int");

                    b.Property<int>("idperiodo")
                        .HasColumnType("int");

                    b.Property<string>("planeado")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("idindicador");

                    b.HasIndex("idperiodo");

                    b.ToTable("Ejecucion");
                });

            modelBuilder.Entity("seguimiento.Models.EjecucionCategoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Calculado")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int");

                    b.Property<decimal>("Maximo")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("Mostrar")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("idperiodo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCategoria");

                    b.HasIndex("idperiodo");

                    b.ToTable("EjecucionCategoria");
                });

            modelBuilder.Entity("seguimiento.Models.Evaluacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Categoriaid")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Contexto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Indicadorid")
                        .HasColumnType("int");

                    b.Property<decimal>("Maximo")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Minimo")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Categoriaid");

                    b.HasIndex("Indicadorid");

                    b.ToTable("Evaluacion");
                });

            modelBuilder.Entity("seguimiento.Models.Indicador", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nota")
                        .HasColumnType("longtext");

                    b.Property<string>("adjunto")
                        .HasColumnType("longtext");

                    b.Property<string>("codigo")
                        .HasColumnType("longtext");

                    b.Property<int>("idCategoria")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .HasColumnType("longtext");

                    b.Property<decimal>("ponderador")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("tipo")
                        .HasColumnType("int");

                    b.Property<string>("unidad")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("idCategoria");

                    b.HasIndex("tipo");

                    b.ToTable("Indicador");
                });

            modelBuilder.Entity("seguimiento.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Accion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ContenidoNew")
                        .HasColumnType("longtext");

                    b.Property<string>("ContenidoOld")
                        .HasColumnType("longtext");

                    b.Property<string>("Tarea")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserMail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("seguimiento.Models.Nivel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("numero")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Nivel");
                });

            modelBuilder.Entity("seguimiento.Models.Nota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Adjunto")
                        .HasColumnType("longtext");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("IdCategoria");

                    b.HasIndex("UserId");

                    b.ToTable("Nota");
                });

            modelBuilder.Entity("seguimiento.Models.Periodo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("EditarEjecucion")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("EditarProgramacion")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Ocultar")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("calculo")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("cargado")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("desplegado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("orden")
                        .HasColumnType("int");

                    b.Property<string>("tipo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Periodo");
                });

            modelBuilder.Entity("seguimiento.Models.PermisoRol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("CategoriasE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("CategoriasV")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ConfiguraciongE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("EjecucionesE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("EjecucionesV")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("IdRol")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IndicadoresE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IndicadoresV")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NivelesE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NotasE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NotasV")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NotificaconesE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("PeriodosE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("PlaneadosE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("PlaneadosV")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ResponsablesE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RolesE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RolesV")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SuperCategoria")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SuperUsuario")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("UsuariosE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("UsuariosV")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("PermisoRol");
                });

            modelBuilder.Entity("seguimiento.Models.Policy", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("claim")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Policy");
                });

            modelBuilder.Entity("seguimiento.Models.Responsable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Editar")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("IdJefe")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IdJefe");

                    b.ToTable("Responsable");
                });

            modelBuilder.Entity("seguimiento.Models.TipoIndicador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<bool>("Enable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Imagendescripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("file")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TipoIndicador");
                });

            modelBuilder.Entity("seguimiento.Models.ValorCampo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CategoriaPadreid")
                        .HasColumnType("int");

                    b.Property<int>("IdCampo")
                        .HasColumnType("int");

                    b.Property<int?>("IndicadorPadreid")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaPadreid");

                    b.HasIndex("IdCampo");

                    b.HasIndex("IndicadorPadreid");

                    b.ToTable("ValorCampo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("seguimiento.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("seguimiento.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("seguimiento.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("seguimiento.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("seguimiento.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("seguimiento.Models.Campo", b =>
                {
                    b.HasOne("seguimiento.Models.Nivel", "NivelPadre")
                        .WithMany()
                        .HasForeignKey("NivelPadreid");

                    b.HasOne("seguimiento.Models.TipoIndicador", "TipoIndicadorPadre")
                        .WithMany()
                        .HasForeignKey("TipoIndicadorPadreId");

                    b.Navigation("NivelPadre");

                    b.Navigation("TipoIndicadorPadre");
                });

            modelBuilder.Entity("seguimiento.Models.Categoria", b =>
                {
                    b.HasOne("seguimiento.Models.Responsable", "Responsable")
                        .WithMany()
                        .HasForeignKey("IdResponsable")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.Categoria", "CategoriaPadre")
                        .WithMany()
                        .HasForeignKey("idCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.Nivel", "Nivel")
                        .WithMany()
                        .HasForeignKey("idNivel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoriaPadre");

                    b.Navigation("Nivel");

                    b.Navigation("Responsable");
                });

            modelBuilder.Entity("seguimiento.Models.Ejecucion", b =>
                {
                    b.HasOne("seguimiento.Models.Indicador", "Indicador")
                        .WithMany()
                        .HasForeignKey("idindicador")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.Periodo", "Periodo")
                        .WithMany()
                        .HasForeignKey("idperiodo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Indicador");

                    b.Navigation("Periodo");
                });

            modelBuilder.Entity("seguimiento.Models.EjecucionCategoria", b =>
                {
                    b.HasOne("seguimiento.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.Periodo", "Periodo")
                        .WithMany()
                        .HasForeignKey("idperiodo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Periodo");
                });

            modelBuilder.Entity("seguimiento.Models.Evaluacion", b =>
                {
                    b.HasOne("seguimiento.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("Categoriaid");

                    b.HasOne("seguimiento.Models.Indicador", "Indicador")
                        .WithMany()
                        .HasForeignKey("Indicadorid");

                    b.Navigation("Categoria");

                    b.Navigation("Indicador");
                });

            modelBuilder.Entity("seguimiento.Models.Indicador", b =>
                {
                    b.HasOne("seguimiento.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("idCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.TipoIndicador", "TipoIndicador")
                        .WithMany()
                        .HasForeignKey("tipo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("TipoIndicador");
                });

            modelBuilder.Entity("seguimiento.Models.Nota", b =>
                {
                    b.HasOne("seguimiento.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.ApplicationUser", "User")
                        .WithMany("Notas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("User");
                });

            modelBuilder.Entity("seguimiento.Models.Responsable", b =>
                {
                    b.HasOne("seguimiento.Models.Responsable", "ResponsableJefe")
                        .WithMany("Hijos")
                        .HasForeignKey("IdJefe");

                    b.Navigation("ResponsableJefe");
                });

            modelBuilder.Entity("seguimiento.Models.ValorCampo", b =>
                {
                    b.HasOne("seguimiento.Models.Categoria", "CategoriaPadre")
                        .WithMany()
                        .HasForeignKey("CategoriaPadreid");

                    b.HasOne("seguimiento.Models.Campo", "CampoPadre")
                        .WithMany()
                        .HasForeignKey("IdCampo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seguimiento.Models.Indicador", "IndicadorPadre")
                        .WithMany()
                        .HasForeignKey("IndicadorPadreid");

                    b.Navigation("CampoPadre");

                    b.Navigation("CategoriaPadre");

                    b.Navigation("IndicadorPadre");
                });

            modelBuilder.Entity("seguimiento.Models.ApplicationUser", b =>
                {
                    b.Navigation("Notas");
                });

            modelBuilder.Entity("seguimiento.Models.Responsable", b =>
                {
                    b.Navigation("Hijos");
                });
#pragma warning restore 612, 618
        }
    }
}
