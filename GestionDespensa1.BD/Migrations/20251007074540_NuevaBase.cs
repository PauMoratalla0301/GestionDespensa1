using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDespensa1.BD.Migrations
{
    /// <inheritdoc />
    public partial class NuevaBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cajas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImporteInicio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Dni = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SaldoPendiente = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    CUIT = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Monto = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IdCaja = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesCaja_Cajas_IdCaja",
                        column: x => x.IdCaja,
                        principalTable: "Cajas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GananciaPorcentaje = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoPendiente = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ventas_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComprasProveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    FechaCompra = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasProveedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprasProveedor_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesVenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenta = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesVenta_Ventas_IdVenta",
                        column: x => x.IdVenta,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCompraProveedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompra = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCompraProveedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesCompraProveedor_ComprasProveedor_IdCompra",
                        column: x => x.IdCompra,
                        principalTable: "ComprasProveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCompraProveedor_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Categoria_UQ",
                table: "Categorias",
                column: "NombreCategoria",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Cliente_UQ",
                table: "Clientes",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComprasProveedor_IdProveedor",
                table: "ComprasProveedor",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCaja_IdCaja",
                table: "DetallesCaja",
                column: "IdCaja");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCompraProveedor_IdCompra",
                table: "DetallesCompraProveedor",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCompraProveedor_IdProducto",
                table: "DetallesCompraProveedor",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_IdProducto",
                table: "DetallesVenta",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesVenta_IdVenta",
                table: "DetallesVenta",
                column: "IdVenta");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdCategoria",
                table: "Productos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "Producto_UQ",
                table: "Productos",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Proveedor_UQ",
                table: "Proveedores",
                column: "CUIT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdCliente",
                table: "Ventas",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesCaja");

            migrationBuilder.DropTable(
                name: "DetallesCompraProveedor");

            migrationBuilder.DropTable(
                name: "DetallesVenta");

            migrationBuilder.DropTable(
                name: "Cajas");

            migrationBuilder.DropTable(
                name: "ComprasProveedor");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
