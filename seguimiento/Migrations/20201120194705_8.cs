using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDDependencia",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true);

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
                name: "PermisoRol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdRol = table.Column<string>(type: "longtext", nullable: false),
                    SuperUsuario = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SuperCategoria = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotasV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotasE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EjecucionesV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EjecucionesE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CategoriasV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CategoriasE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PlaneadosV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PlaneadosE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IndicadoresV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IndicadoresE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuariosV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuariosE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RolesV = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RolesE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NivelesE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConfiguraciongE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PeriodosE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ResponsablesE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotificaconesE = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisoRol", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Campo_NivelPadreid",
                table: "Campo",
                column: "NivelPadreid");

            migrationBuilder.CreateIndex(
                name: "IX_Campo_TipoIndicadorPadreId",
                table: "Campo",
                column: "TipoIndicadorPadreId");

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
                name: "PermisoRol");

            migrationBuilder.DropTable(
                name: "ValorCampo");

            migrationBuilder.DropTable(
                name: "Periodo");

            migrationBuilder.DropTable(
                name: "Campo");

            migrationBuilder.DropTable(
                name: "Indicador");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IDDependencia",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");
        }
    }
}
