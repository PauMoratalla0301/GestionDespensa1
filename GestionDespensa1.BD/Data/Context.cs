using GestionDespensa1.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        // Define tus DbSet<T> aquí según tus entidades
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<DetalleCaja> DetallesCaja { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<CompraProveedor> ComprasProveedor { get; set; }
        public DbSet<DetalleCompraProveedor> DetallesCompraProveedor { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de precisiones para campos decimales
            ConfigureDecimalPrecisions(modelBuilder);
        }

        private void ConfigureDecimalPrecisions(ModelBuilder modelBuilder)
        {
            // Caja
            modelBuilder.Entity<Caja>()
                .Property(c => c.ImporteInicio)
                .HasPrecision(18, 2);

            // Cliente
            modelBuilder.Entity<Cliente>()
                .Property(c => c.SaldoPendiente)
                .HasPrecision(18, 2);

            // DetalleVenta
            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            // Producto
            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.GananciaPorcentaje)
                .HasPrecision(5, 2); // Porcentaje: 100.00%

            // Venta
            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.MontoPagado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.SaldoPendiente)
                .HasPrecision(18, 2);

            // DetalleCompraProveedor
            modelBuilder.Entity<DetalleCompraProveedor>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);
        }
    }
}
