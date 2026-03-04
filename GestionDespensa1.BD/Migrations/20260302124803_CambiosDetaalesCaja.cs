using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDespensa1.BD.Migrations
{
    /// <inheritdoc />
    public partial class CambiosDetaalesCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdVenta",
                table: "DetallesCaja");

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "DetallesCaja",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AddColumn<string>(
                name: "Concepto",
                table: "DetallesCaja",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "DetallesCaja",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "DetallesCaja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "DetallesCaja",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concepto",
                table: "DetallesCaja");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "DetallesCaja");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "DetallesCaja");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "DetallesCaja");

            migrationBuilder.AlterColumn<string>(
                name: "Monto",
                table: "DetallesCaja",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "IdVenta",
                table: "DetallesCaja",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");
        }
    }
}
