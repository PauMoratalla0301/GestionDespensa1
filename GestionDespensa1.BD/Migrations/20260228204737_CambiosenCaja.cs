using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDespensa1.BD.Migrations
{
    /// <inheritdoc />
    public partial class CambiosenCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajas_Usuarios_UsuarioId",
                table: "Cajas");

            migrationBuilder.DropIndex(
                name: "IX_Cajas_UsuarioId",
                table: "Cajas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Cajas");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "Cajas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45);

            migrationBuilder.CreateIndex(
                name: "IX_Cajas_IdUsuario",
                table: "Cajas",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajas_Usuarios_IdUsuario",
                table: "Cajas",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cajas_Usuarios_IdUsuario",
                table: "Cajas");

            migrationBuilder.DropIndex(
                name: "IX_Cajas_IdUsuario",
                table: "Cajas");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Cajas",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Cajas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cajas_UsuarioId",
                table: "Cajas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cajas_Usuarios_UsuarioId",
                table: "Cajas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
