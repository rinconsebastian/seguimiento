using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: true),
                    Apellido = table.Column<string>(type: "longtext", nullable: true),
                    IDDependencia = table.Column<int>(type: "int", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    anoInicial = table.Column<int>(type: "int", nullable: false),
                    anoFinal = table.Column<int>(type: "int", nullable: false),
                    periodosAnuales = table.Column<int>(type: "int", nullable: false),
                    nombrePeriodoAnual = table.Column<string>(type: "longtext", nullable: false),
                    Logo = table.Column<string>(type: "longtext", nullable: true),
                    ImgHeader = table.Column<string>(type: "longtext", nullable: true),
                    ImgBackgroud = table.Column<string>(type: "longtext", nullable: true),
                    colorTextoHeader = table.Column<string>(type: "longtext", nullable: true),
                    colorPrincipal = table.Column<string>(type: "longtext", nullable: true),
                    colorTextoPrincipal = table.Column<string>(type: "longtext", nullable: true),
                    contacto = table.Column<string>(type: "longtext", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Entidad = table.Column<string>(type: "longtext", nullable: false),
                    NombrePlan = table.Column<string>(type: "longtext", nullable: false),
                    PonderacionTipo = table.Column<string>(type: "longtext", nullable: false),
                    CalculoNivel = table.Column<string>(type: "longtext", nullable: false),
                    libre = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EstiloReporte = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "longtext", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    UserMail = table.Column<string>(type: "longtext", nullable: false),
                    Tarea = table.Column<string>(type: "longtext", nullable: false),
                    Accion = table.Column<string>(type: "longtext", nullable: false),
                    ContenidoNew = table.Column<string>(type: "longtext", nullable: true),
                    ContenidoOld = table.Column<string>(type: "longtext", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nivel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "longtext", nullable: false),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nivel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Periodo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    orden = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "longtext", nullable: false),
                    tipo = table.Column<string>(type: "longtext", nullable: false),
                    calculo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    cargado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EditarEjecucion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EditarProgramacion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Ocultar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    desplegado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false),
                    claim = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Responsable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdJefe = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "longtext", nullable: false),
                    Editar = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsable_Responsable_IdJefe",
                        column: x => x.IdJefe,
                        principalTable: "Responsable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TipoIndicador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<string>(type: "longtext", nullable: false),
                    file = table.Column<string>(type: "longtext", nullable: false),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true),
                    Imagendescripcion = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoIndicador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idCategoria = table.Column<int>(type: "int", nullable: true),
                    idNivel = table.Column<int>(type: "int", nullable: false),
                    IdResponsable = table.Column<int>(type: "int", nullable: false),
                    numero = table.Column<string>(type: "longtext", nullable: true),
                    nombre = table.Column<string>(type: "longtext", nullable: false),
                    unificacion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Ponderador = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    texto = table.Column<string>(type: "longtext", nullable: true),
                    objetivo = table.Column<string>(type: "longtext", nullable: true),
                    nota = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_Categoria_Categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categoria_Nivel_idNivel",
                        column: x => x.idNivel,
                        principalTable: "Nivel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categoria_Responsable_IdResponsable",
                        column: x => x.IdResponsable,
                        principalTable: "Responsable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NivelPadreid = table.Column<int>(type: "int", nullable: true),
                    TipoIndicadorPadreId = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "longtext", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false),
                    Activado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TodoIndicador = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TodaCategoria = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campo_Nivel_NivelPadreid",
                        column: x => x.NivelPadreid,
                        principalTable: "Nivel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Campo_TipoIndicador_TipoIndicadorPadreId",
                        column: x => x.TipoIndicadorPadreId,
                        principalTable: "TipoIndicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EjecucionCategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    idperiodo = table.Column<int>(type: "int", nullable: false),
                    Calculado = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Maximo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Mostrar = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjecucionCategoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EjecucionCategoria_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EjecucionCategoria_Periodo_idperiodo",
                        column: x => x.idperiodo,
                        principalTable: "Periodo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indicador",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
                    ponderador = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    codigo = table.Column<string>(type: "longtext", nullable: true),
                    unidad = table.Column<string>(type: "longtext", nullable: false),
                    nombre = table.Column<string>(type: "longtext", nullable: true),
                    Nota = table.Column<string>(type: "longtext", nullable: true),
                    adjunto = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicador", x => x.id);
                    table.ForeignKey(
                        name: "FK_Indicador_Categoria_idCategoria",
                        column: x => x.idCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Indicador_TipoIndicador_tipo",
                        column: x => x.tipo,
                        principalTable: "TipoIndicador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(type: "longtext", nullable: false),
                    Texto = table.Column<string>(type: "longtext", nullable: false),
                    Estado = table.Column<string>(type: "longtext", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Adjunto = table.Column<string>(type: "longtext", nullable: true),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nota_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nota_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ejecucion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idindicador = table.Column<int>(type: "int", nullable: false),
                    idperiodo = table.Column<int>(type: "int", nullable: false),
                    planeado = table.Column<string>(type: "longtext", nullable: true),
                    ejecutado = table.Column<string>(type: "longtext", nullable: true),
                    cargado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Nota = table.Column<string>(type: "longtext", nullable: true),
                    adjunto = table.Column<string>(type: "longtext", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejecucion", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ejecucion_Indicador_idindicador",
                        column: x => x.idindicador,
                        principalTable: "Indicador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ejecucion_Periodo_idperiodo",
                        column: x => x.idperiodo,
                        principalTable: "Periodo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false),
                    Color = table.Column<string>(type: "longtext", nullable: false),
                    Minimo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Maximo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Contexto = table.Column<string>(type: "longtext", nullable: false),
                    Indicadorid = table.Column<int>(type: "int", nullable: true),
                    Categoriaid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Categoria_Categoriaid",
                        column: x => x.Categoriaid,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Indicador_Indicadorid",
                        column: x => x.Indicadorid,
                        principalTable: "Indicador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ValorCampo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCampo = table.Column<int>(type: "int", nullable: false),
                    IndicadorPadreid = table.Column<int>(type: "int", nullable: true),
                    CategoriaPadreid = table.Column<int>(type: "int", nullable: true),
                    Texto = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorCampo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValorCampo_Campo_IdCampo",
                        column: x => x.IdCampo,
                        principalTable: "Campo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValorCampo_Categoria_CategoriaPadreid",
                        column: x => x.CategoriaPadreid,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValorCampo_Indicador_IndicadorPadreid",
                        column: x => x.IndicadorPadreid,
                        principalTable: "Indicador",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", "97f6ff5b-6816-44fc-8e6f-bbdedd1223f9", "Administrador", "ADMINISTRADOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Apellido", "ConcurrencyStamp", "Email", "EmailConfirmed", "IDDependencia", "LockoutEnabled", "LockoutEnd", "Nombre", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "", "05622443-5cfd-4389-8879-4523ac4c5aee", "admin@admin.com", true, 1, false, null, "Admin", "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAECPDxHYYnrFlyL6ghv6NFqs7g9ZlRCuHRIgzChzRa5GDZpnwsj563VfwncgzZt+OTw==", null, false, "NNK44MKHKTBOV6DHXJ4BT2Q3SYO3WQC2", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "Configuracion",
                columns: new[] { "id", "CalculoNivel", "Entidad", "EstiloReporte", "ImgBackgroud", "ImgHeader", "Logo", "NombrePlan", "PonderacionTipo", "activo", "anoFinal", "anoInicial", "colorPrincipal", "colorTextoHeader", "colorTextoPrincipal", "contacto", "libre", "nombrePeriodoAnual", "periodosAnuales" },
                values: new object[] { 1, "2", "Entidad", "", null, null, "/images/SIE.png", "Plan", "PonderacionAbsoluta", true, 2021, 2020, "#52a3a1", "#ffffff", "#00000", "rinconsebastian@gmail.com", true, "Trimestre", 4 });

            migrationBuilder.InsertData(
                table: "Evaluacion",
                columns: new[] { "Id", "Categoriaid", "Color", "Contexto", "Indicadorid", "Maximo", "Minimo", "Nombre" },
                values: new object[,]
                {
                    { 1, null, "#ff0000", "Global", null, 60m, 0m, "Mínimo" },
                    { 2, null, "#ffff00", "Global", null, 80m, 60m, "Aceptable" },
                    { 3, null, "#00ff00", "Global", null, 100m, 80m, "Satisfactorio" }
                });

            migrationBuilder.InsertData(
                table: "Nivel",
                columns: new[] { "id", "color", "nombre", "numero" },
                values: new object[,]
                {
                    { 1, "000000", "Plan", 1 },
                    { 2, "000000", "Categoria", 2 }
                });

            migrationBuilder.InsertData(
                table: "Policy",
                columns: new[] { "id", "claim", "nombre" },
                values: new object[,]
                {
                    { 15, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar", "Editar notas" },
                    { 14, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs", "Ver registro actividad" },
                    { 13, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar", "Editar usuarios" },
                    { 12, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar", "Editar roles" },
                    { 11, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar", "Editar evaluaciones" },
                    { 10, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar", "Editar campos" },
                    { 9, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar", "Editar niveles" },
                    { 5, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar", "Editar indicadores" },
                    { 7, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", "Editar categorias" },
                    { 6, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar", "Editar periodo" },
                    { 4, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar", "Editar planeación" },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar", "Editar ejecución" },
                    { 2, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable", "Configuración dependencia" },
                    { 1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", "Ver Configuración general" },
                    { 8, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar", "Editar dependencias" }
                });

            migrationBuilder.InsertData(
                table: "Responsable",
                columns: new[] { "Id", "Editar", "IdJefe", "Nombre" },
                values: new object[] { 1, true, null, "Entidad" });

            migrationBuilder.InsertData(
                table: "TipoIndicador",
                columns: new[] { "Id", "Descripcion", "Enable", "Imagendescripcion", "Tipo", "file" },
                values: new object[] { 1, "AIncnoacumTmtto", true, null, "AIncnoacumTmtto", "AIncnoacumTmtto" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", "1", "1" },
                    { 15, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar", "1", "1" },
                    { 14, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs", "1", "1" },
                    { 13, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar", "1", "1" },
                    { 12, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar", "1", "1" },
                    { 11, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar", "1", "1" },
                    { 10, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar", "1", "1" },
                    { 9, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar", "1", "1" },
                    { 7, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", "1", "1" },
                    { 6, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar", "1", "1" },
                    { 5, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar", "1", "1" },
                    { 4, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar", "1", "1" },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar", "1", "1" },
                    { 2, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable", "1", "1" },
                    { 8, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar", "1", "1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "id", "IdResponsable", "Ponderador", "idCategoria", "idNivel", "nombre", "nota", "numero", "objetivo", "texto", "unificacion" },
                values: new object[] { 1, 1, 1m, null, 1, "Principal", null, "0.", null, null, true });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campo_NivelPadreid",
                table: "Campo",
                column: "NivelPadreid");

            migrationBuilder.CreateIndex(
                name: "IX_Campo_TipoIndicadorPadreId",
                table: "Campo",
                column: "TipoIndicadorPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_idCategoria",
                table: "Categoria",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_idNivel",
                table: "Categoria",
                column: "idNivel");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdResponsable",
                table: "Categoria",
                column: "IdResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_Ejecucion_idindicador",
                table: "Ejecucion",
                column: "idindicador");

            migrationBuilder.CreateIndex(
                name: "IX_Ejecucion_idperiodo",
                table: "Ejecucion",
                column: "idperiodo");

            migrationBuilder.CreateIndex(
                name: "IX_EjecucionCategoria_IdCategoria",
                table: "EjecucionCategoria",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_EjecucionCategoria_idperiodo",
                table: "EjecucionCategoria",
                column: "idperiodo");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Categoriaid",
                table: "Evaluacion",
                column: "Categoriaid");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_Indicadorid",
                table: "Evaluacion",
                column: "Indicadorid");

            migrationBuilder.CreateIndex(
                name: "IX_Indicador_idCategoria",
                table: "Indicador",
                column: "idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Indicador_tipo",
                table: "Indicador",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdCategoria",
                table: "Nota",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_UserId",
                table: "Nota",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsable_IdJefe",
                table: "Responsable",
                column: "IdJefe");

            migrationBuilder.CreateIndex(
                name: "IX_ValorCampo_CategoriaPadreid",
                table: "ValorCampo",
                column: "CategoriaPadreid");

            migrationBuilder.CreateIndex(
                name: "IX_ValorCampo_IdCampo",
                table: "ValorCampo",
                column: "IdCampo");

            migrationBuilder.CreateIndex(
                name: "IX_ValorCampo_IndicadorPadreid",
                table: "ValorCampo",
                column: "IndicadorPadreid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Configuracion");

            migrationBuilder.DropTable(
                name: "Ejecucion");

            migrationBuilder.DropTable(
                name: "EjecucionCategoria");

            migrationBuilder.DropTable(
                name: "Evaluacion");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Policy");

            migrationBuilder.DropTable(
                name: "ValorCampo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Periodo");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Campo");

            migrationBuilder.DropTable(
                name: "Indicador");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "TipoIndicador");

            migrationBuilder.DropTable(
                name: "Nivel");

            migrationBuilder.DropTable(
                name: "Responsable");
        }
    }
}
