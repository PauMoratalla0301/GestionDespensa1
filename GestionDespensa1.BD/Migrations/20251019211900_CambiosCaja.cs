using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDespensa1.BD.Migrations
{
    /// <inheritdoc />
    public partial class CambiosCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Cajas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ImporteCierre",
                table: "Cajas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Cajas",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "ImporteCierre",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Cajas");
        }
    }
}
