using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class data_policy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Policy",
                columns: new[] { "id", "claim", "nombre" },
                values: new object[,]
                {
                    { 1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.General", "Ver Configuración general" },
                    { 2, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Responsable", "Configuración dependencia" },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Ejecucion.Editar", "Editar ejecución" },
                    { 4, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Planeacion.Editar", "Editar planeación" },
                    { 5, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Indicador.Editar", "Editar indicadores" },
                    { 6, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Periodo.Editar", "Editar periodo" },
                    { 7, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Categoria.Editar", "Editar categorias" },
                    { 8, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Responsable.Editar", "Editar dependencias" },
                    { 9, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nivel.Editar", "Editar niveles" },
                    { 10, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Campo.Editar", "Editar campos" },
                    { 11, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Evaluacion.Editar", "Editar evaluaciones" },
                    { 12, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Rol.Editar", "Editar roles" },
                    { 13, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Usuario.Editar", "Editar usuarios" },
                    { 14, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Configuracion.Logs", "Ver registro actividad" },
                    { 15, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Nota.Editar", "Editar notas" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Policy",
                keyColumn: "id",
                keyValue: 15);
        }
    }
}
