using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Servicios
{
    public class ResumenCajaService
    {
        private readonly Context _context;

        public ResumenCajaService(Context context)
        {
            _context = context;
        }

        public async Task<ResumenVentasDTO> GetResumenPorFecha(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fecha.Date.AddDays(1).AddSeconds(-1);

            // 1. VENTAS del día (solo pagadas o completadas)
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin
                    && (v.Estado == "Pagado" || v.Estado == "Completada"))
                .ToListAsync();

            // 2. PAGOS de clientes (deudas anteriores)
            var pagosClientes = await _context.PagosVenta
                .Include(p => p.Venta)
                .Where(p => p.Fecha >= fechaInicio && p.Fecha <= fechaFin)
                .ToListAsync();

            // 3. PAGOS a proveedores
            var pagosProveedores = await _context.PagosProveedor
                .Include(p => p.Compra)
                    .ThenInclude(c => c.Proveedor)
                .Where(p => p.Fecha >= fechaInicio && p.Fecha <= fechaFin)
                .ToListAsync();

            // 4. GASTOS (egresos de caja)
            var gastos = await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.Fecha >= fechaInicio && d.Fecha <= fechaFin &&
                           d.Tipo == "EGRESO" &&
                           !d.Concepto.Contains("Compra"))
                .ToListAsync();

            // Buscar caja del día
            var caja = await _context.Cajas
                .FirstOrDefaultAsync(c => c.Fecha.Date == fecha.Date);

            var resumen = new ResumenVentasDTO
            {
                // Ventas del día
                TotalVentas = ventas.Sum(v => v.Total),
                CantidadVentas = ventas.Count,

                // Desglose de ventas por medio de pago
                TotalEfectivo = ventas.Where(v => v.MetodoPago.Contains("Efectivo")).Sum(v => v.Total),
                TotalTarjeta = ventas.Where(v => v.MetodoPago.Contains("Tarjeta")).Sum(v => v.Total),
                TotalTransferencia = ventas.Where(v => v.MetodoPago.Contains("Transferencia")).Sum(v => v.Total),

                // Pagos de clientes (deudas)
                PagosClientesEfectivo = pagosClientes.Where(p => p.MedioPago == "Efectivo").Sum(p => p.Monto),
                PagosClientesTransferencia = pagosClientes.Where(p => p.MedioPago == "Transferencia").Sum(p => p.Monto),
                PagosClientesTarjeta = pagosClientes.Where(p => p.MedioPago == "Tarjeta").Sum(p => p.Monto),

                // Pagos a proveedores
                PagosProveedoresEfectivo = pagosProveedores.Where(p => p.MedioPago == "EFECTIVO").Sum(p => p.Monto),
                PagosProveedoresTransferencia = pagosProveedores.Where(p => p.MedioPago == "TRANSFERENCIA").Sum(p => p.Monto),

                // Gastos varios
                GastosEfectivo = gastos.Where(g => g.Concepto.Contains("EFECTIVO")).Sum(g => g.Monto),
                GastosTransferencia = gastos.Where(g => g.Concepto.Contains("TRANSFERENCIA")).Sum(g => g.Monto),

                // Detalle de egresos
                DetalleEgresos = pagosProveedores.Select(p => new DetalleEgresoDTO
                {
                    Id = p.Id,
                    Fecha = p.Fecha,
                    Concepto = $"Pago a proveedor: {p.Compra?.Proveedor?.Nombre}",
                    Monto = p.Monto,
                    MedioPago = p.MedioPago,
                    Tipo = "PROVEEDOR",
                    Referencia = $"Compra #{p.IdCompra}",
                    Proveedor = p.Compra?.Proveedor?.Nombre
                }).Concat(gastos.Select(g => new DetalleEgresoDTO
                {
                    Id = g.Id,
                    Fecha = g.Fecha,
                    Concepto = g.Concepto,
                    Monto = g.Monto,
                    MedioPago = "EFECTIVO",
                    Tipo = "GASTO",
                    Referencia = g.Referencia
                })).OrderByDescending(e => e.Fecha).ToList(),

                // Caja
                ImporteInicio = caja?.ImporteInicio ?? 0,
                ImporteCierre = caja?.ImporteCierre
            };

            resumen.TotalEgresos = resumen.DetalleEgresos.Sum(e => e.Monto);

            return resumen;
        }
    }
}