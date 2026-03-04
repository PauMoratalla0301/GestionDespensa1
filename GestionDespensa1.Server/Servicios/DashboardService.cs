using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Servicios
{
    public class DashboardService
    {
        private readonly Context _context;

        public DashboardService(Context context)
        {
            _context = context;
        }

        public async Task<DashboardDTO> ObtenerDashboard()
        {
            var hoy = DateTime.Today;
            var primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);
            var sieteDiasAtras = hoy.AddDays(-7);

            var dashboard = new DashboardDTO();

            // 1. Ventas del día
            var ventasHoy = await _context.Ventas
                .Where(v => v.FechaVenta.Date == hoy.Date && v.Estado != "Cancelado")
                .ToListAsync();

            dashboard.VentasHoy = ventasHoy.Sum(v => v.Total);
            dashboard.CantidadVentasHoy = ventasHoy.Count;

            // 2. Ventas del mes
            var ventasMes = await _context.Ventas
                .Where(v => v.FechaVenta >= primerDiaMes && v.Estado != "Cancelado")
                .ToListAsync();

            dashboard.VentasMes = ventasMes.Sum(v => v.Total);
            dashboard.CantidadVentasMes = ventasMes.Count;

            // 3. Productos con stock bajo
            dashboard.ProductosStockBajo = await _context.Productos
                .CountAsync(p => p.StockActual <= p.StockMinimo);

            dashboard.TotalProductos = await _context.Productos.CountAsync();

            // 4. Estado de caja
            var cajaHoy = await _context.Cajas
                .Where(c => c.Fecha.Date == hoy.Date && c.Estado == "Abierta")
                .FirstOrDefaultAsync();

            dashboard.CajaAbierta = cajaHoy != null;
            dashboard.CajaInicio = cajaHoy?.ImporteInicio;

            if (cajaHoy != null)
            {
                var ventasCaja = await _context.Ventas
                    .Where(v => v.FechaVenta.Date == hoy.Date && v.Estado == "Pagado")
                    .SumAsync(v => v.Total);
                dashboard.CajaActual = cajaHoy.ImporteInicio + ventasCaja;
            }

            // 5. Clientes deudores
            dashboard.TotalClientesDeudores = await _context.Clientes
                .Where(c => c.SaldoPendiente > 0)
                .SumAsync(c => c.SaldoPendiente);

            dashboard.CantidadClientesDeudores = await _context.Clientes
                .CountAsync(c => c.SaldoPendiente > 0);

            // 6. Compras pendientes
            dashboard.TotalComprasPendientes = await _context.ComprasProveedor
                .Where(c => c.Estado == "PENDIENTE")
                .SumAsync(c => c.Total);

            // 7. Últimas ventas
            dashboard.UltimasVentas = await _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.Estado != "Cancelado")
                .OrderByDescending(v => v.FechaVenta)
                .Take(5)
                .Select(v => new UltimaVentaDTO
                {
                    Id = v.Id,
                    Fecha = v.FechaVenta,
                    Cliente = v.Cliente != null ? v.Cliente.Nombre + " " + v.Cliente.Apellido : "Consumidor Final",
                    Total = v.Total,
                    Estado = v.Estado
                })
                .ToListAsync();

            // 8. Últimos movimientos de stock
            dashboard.UltimosMovimientosStock = await _context.MovimientosStock
                .Include(m => m.Producto)
                .OrderByDescending(m => m.Fecha)
                .Take(5)
                .Select(m => new UltimoMovimientoStockDTO
                {
                    Id = m.Id,
                    Producto = m.Producto != null ? m.Producto.Descripcion : "",
                    Tipo = m.Tipo,
                    Cantidad = m.Cantidad,
                    Fecha = m.Fecha,
                    Referencia = m.Referencia ?? ""
                })
                .ToListAsync();

            // 9. Productos más vendidos (últimos 30 días)
            var mesPasado = hoy.AddDays(-30);
            var detallesVentas = await _context.DetallesVenta
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .Where(d => d.Venta.FechaVenta >= mesPasado && d.Venta.Estado != "Cancelado")
                .ToListAsync();

            dashboard.ProductosMasVendidos = detallesVentas
                .GroupBy(d => d.Producto != null ? d.Producto.Descripcion : "Sin producto")
                .Select(g => new ProductoMasVendidoDTO
                {
                    Producto = g.Key,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVendido = g.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .Take(5)
                .ToList();

            // 10. Ventas últimos 7 días
            for (int i = 6; i >= 0; i--)
            {
                var dia = hoy.AddDays(-i);
                var ventasDia = await _context.Ventas
                    .Where(v => v.FechaVenta.Date == dia.Date && v.Estado != "Cancelado")
                    .ToListAsync();

                dashboard.VentasUltimos7Dias.Add(new VentaDiariaDTO
                {
                    Dia = dia,
                    DiaSemana = dia.ToString("dddd"),
                    Total = ventasDia.Sum(v => v.Total),
                    Cantidad = ventasDia.Count
                });
            }

            return dashboard;
        }
    }
}