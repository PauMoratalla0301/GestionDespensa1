using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Servicios;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprobantesVentaController : ControllerBase
    {
        private readonly Context _context;
        private readonly PdfVentaService _pdfVentaService;

        public ComprobantesVentaController(Context context, PdfVentaService pdfVentaService)
        {
            _context = context;
            _pdfVentaService = pdfVentaService;
        }

        [HttpGet("venta/{id:int}")]
        public async Task<IActionResult> DescargarComprobanteVenta(int id)
        {
            try
            {
                // Obtener venta con relaciones
                var venta = await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.DetallesVenta)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venta == null)
                    return NotFound("Venta no encontrada");

                // Crear DTO
                var ventaDTO = new VentaDTO
                {
                    Id = venta.Id,
                    IdCliente = venta.IdCliente,
                    IdUsuario = venta.IdUsuario,
                    FechaVenta = venta.FechaVenta,
                    Estado = venta.Estado,
                    Total = venta.Total,
                    MontoPagado = venta.MontoPagado,
                    SaldoPendiente = venta.SaldoPendiente,
                    MetodoPago = venta.MetodoPago,
                    Notas = venta.Notas
                };

                var detalles = venta.DetallesVenta?.Select(d => new DetalleVentaDTO
                {
                    Id = d.Id,
                    IdVenta = d.IdVenta,
                    IdProducto = d.IdProducto,
                    DescripcionProducto = d.Producto?.Descripcion ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList() ?? new();

                string nombreCliente = venta.Cliente != null
                    ? $"{venta.Cliente.Nombre} {venta.Cliente.Apellido}"
                    : "Consumidor Final";

                // Generar PDF
                byte[] pdfBytes = _pdfVentaService.GenerarComprobanteVenta(ventaDTO, detalles, nombreCliente);

                // Devolver archivo
                string nombreArchivo = $"Venta_{id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar comprobante: {ex.Message}");
            }
        }
    }
}