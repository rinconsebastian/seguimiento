using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class adjunto_ejecucion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EjecucionAdjunto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idejecucion = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "longtext", nullable: true),
                    adjunto = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjecucionAdjunto", x => x.id);
                    table.ForeignKey(
                        name: "FK_EjecucionAdjunto_Ejecucion_idejecucion",
                        column: x => x.idejecucion,
                        principalTable: "Ejecucion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EjecucionAdjunto_idejecucion",
                table: "EjecucionAdjunto",
                column: "idejecucion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EjecucionAdjunto");
        }
    }
}
