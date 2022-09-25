using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace seguimiento.Migrations
{
    public partial class accionesCCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Accion1",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accion2",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accion3",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cerrada1",
                table: "NotaIndicador",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cerrada2",
                table: "NotaIndicador",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cerrada3",
                table: "NotaIndicador",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion1",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion2",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion3",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinal1",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinal2",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinal3",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio1",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio2",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio3",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSeg1",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSeg2",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSeg3",
                table: "NotaIndicador",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Responsable1",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsable2",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsable3",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soporte1",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soporte2",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soporte3",
                table: "NotaIndicador",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accion1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Accion2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Accion3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Cerrada1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Cerrada2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Cerrada3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Descripcion1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Descripcion2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Descripcion3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaFinal1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaFinal2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaFinal3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaInicio1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaInicio2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaInicio3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaSeg1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaSeg2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "FechaSeg3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Responsable1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Responsable2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Responsable3",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Soporte1",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Soporte2",
                table: "NotaIndicador");

            migrationBuilder.DropColumn(
                name: "Soporte3",
                table: "NotaIndicador");
        }
    }
}
