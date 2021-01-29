using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class colores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "colorPrincipal",
                table: "Configuracion",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "colorTextoHeader",
                table: "Configuracion",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "colorTextoPrincipal",
                table: "Configuracion",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "colorPrincipal",
                table: "Configuracion");

            migrationBuilder.DropColumn(
                name: "colorTextoHeader",
                table: "Configuracion");

            migrationBuilder.DropColumn(
                name: "colorTextoPrincipal",
                table: "Configuracion");
        }
    }
}
