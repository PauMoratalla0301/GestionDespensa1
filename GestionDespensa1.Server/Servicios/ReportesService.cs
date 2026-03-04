using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Servicios
{
    public class ReportesService
    {
        private readonly Context _context;

        public ReportesService(Context context)
        {
            _context = context;
        }

        public async Task<ReporteVentasDTO> GetVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.FechaVenta.Date >= fechaInicio.Date &&
                           v.FechaVenta.Date <= fechaFin.Date &&
                           v.Estado != "Cancelado")
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return new ReporteVentasDTO
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TotalVentas = ventas.Count,
                TotalIngresos = ventas.Sum(v => v.Total),
                TotalEfectivo = ventas.Where(v => v.MetodoPago == "Efectivo").Sum(v => v.Total),
                TotalTarjeta = ventas.Where(v => v.MetodoPago == "Tarjeta").Sum(v => v.Total),
                TotalTransferencia = ventas.Where(v => v.MetodoPago == "Transferencia").Sum(v => v.Total),
                Ventas = ventas.Select(v => new VentaResumenDTO
                {
                    Id = v.Id,
                    Fecha = v.FechaVenta,
                    Cliente = v.Cliente != null ? v.Cliente.Nombre + " " + v.Cliente.Apellido : "Consumidor Final",
                    Total = v.Total,
                    Estado = v.Estado,
                    MetodoPago = v.MetodoPago
                }).ToList()
            };
        }

        public async Task<ReporteProductosDTO> GetProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            var detalles = await _context.DetallesVenta
                .Include(d => d.Producto)
                    .ThenInclude(p => p.Categoria)
                .Include(d => d.Venta)
                .Where(d => d.Venta.FechaVenta.Date >= fechaInicio.Date &&
                           d.Venta.FechaVenta.Date <= fechaFin.Date &&
                           d.Venta.Estado != "Cancelado")
                .ToListAsync();

            var productos = detalles
                .GroupBy(d => new { d.IdProducto, d.Producto.Descripcion, Categoria = d.Producto.Categoria.NombreCategoria })
                .Select(g => new ProductoVendidoDTO
                {
                    IdProducto = g.Key.IdProducto,
                    Producto = g.Key.Descripcion,
                    Categoria = g.Key.Categoria,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVendido = g.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .ToList();

            return new ReporteProductosDTO
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Productos = productos
            };
        }

        public async Task<ReporteDeudoresDTO> GetClientesDeudores()
        {
            var clientes = await _context.Clientes
                .Where(c => c.SaldoPendiente > 0)
                .ToListAsync();

            var deudores = new List<ClienteDeudorDTO>();

            foreach (var cliente in clientes)
            {
                var ventas = await _context.Ventas
                    .Where(v => v.IdCliente == cliente.Id && v.SaldoPendiente > 0)
                    .OrderByDescending(v => v.FechaVenta)
                    .ToListAsync();

                deudores.Add(new ClienteDeudorDTO
                {
                    IdCliente = cliente.Id,
                    Cliente = cliente.Nombre + " " + cliente.Apellido,
                    Telefono = cliente.Telefono ?? "",
                    Email = cliente.Email ?? "",
                    SaldoPendiente = cliente.SaldoPendiente,
                    Ventas = ventas.Select(v => new VentaDeudorDTO
                    {
                        IdVenta = v.Id,
                        Fecha = v.FechaVenta,
                        Total = v.Total,
                        Pagado = v.MontoPagado,
                        Saldo = v.SaldoPendiente
                    }).ToList()
                });
            }

            return new ReporteDeudoresDTO
            {
                Deudores = deudores,
                TotalGeneral = deudores.Sum(d => d.SaldoPendiente),
                CantidadDeudores = deudores.Count
            };
        }

        public async Task<ReporteStockBajoDTO> GetStockBajo()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.StockActual <= p.StockMinimo)
                .OrderBy(p => p.StockActual)
                .ToListAsync();

            return new ReporteStockBajoDTO
            {
                Productos = productos.Select(p => new ProductoStockBajoDTO
                {
                    IdProducto = p.Id,
                    Producto = p.Descripcion,
                    Categoria = p.Categoria?.NombreCategoria ?? "",
                    StockActual = p.StockActual,
                    StockMinimo = p.StockMinimo
                }).ToList(),
                TotalProductos = await _context.Productos.CountAsync()
            };
        }
    }
}