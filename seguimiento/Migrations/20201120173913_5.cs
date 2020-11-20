using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    contacto = table.Column<string>(type: "longtext", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Entidad = table.Column<string>(type: "longtext", nullable: false),
                    NombrePlan = table.Column<string>(type: "longtext", nullable: false),
                    PonderacionTipo = table.Column<string>(type: "longtext", nullable: false),
                    CalculoNivel = table.Column<string>(type: "longtext", nullable: false),
                    libre = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracion", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracion");
        }
    }
}
