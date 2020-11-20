using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idCategoria = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
