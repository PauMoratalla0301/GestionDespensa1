using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Servicios
{
    public class NotificacionesService
    {
        private readonly Context _context;

        public NotificacionesService(Context context)
        {
            _context = context;
        }

        public async Task<NotificacionesDTO> ObtenerNotificaciones()
        {
            var notificaciones = new NotificacionesDTO();

            // 1. Productos con stock bajo
            notificaciones.StockBajo = await _context.Productos
                .CountAsync(p => p.StockActual <= p.StockMinimo);

            // 2. Clientes deudores
            notificaciones.ClientesDeudores = await _context.Clientes
                .CountAsync(c => c.SaldoPendiente > 0);

            // 3. Compras pendientes de pago
            notificaciones.ComprasPendientes = await _context.ComprasProveedor
                .CountAsync(c => c.Estado == "PENDIENTE");

            // 4. Ventas con saldo pendiente (opcional)
            notificaciones.VentasPendientes = await _context.Ventas
                .CountAsync(v => v.SaldoPendiente > 0);

            return notificaciones;
        }
    }
}