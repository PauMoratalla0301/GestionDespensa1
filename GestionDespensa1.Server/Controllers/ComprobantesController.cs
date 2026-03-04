using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Servicios;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprobantesController : ControllerBase
    {
        private readonly Context _context;
        private readonly PdfService _pdfService;

        public ComprobantesController(Context context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        [HttpGet("compra/{id:int}")]
        public async Task<IActionResult> DescargarComprobanteCompra(int id)
        {
            try
            {
                // Obtener compra
                var compra = await _context.ComprasProveedor
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compra == null)
                    return NotFound("Compra no encontrada");

                // Obtener pagos
                var pagos = await _context.PagosProveedor
                    .Where(p => p.IdCompra == id)
                    .ToListAsync();

                decimal totalPagado = pagos.Sum(p => p.Monto);

                // Crear DTO
                var compraDTO = new CompraProveedorDTO
                {
                    Id = compra.Id,
                    IdProveedor = compra.IdProveedor,
                    NombreProveedor = compra.Proveedor?.Nombre ?? "",
                    FechaCompra = compra.FechaCompra,
                    Total = compra.DetallesCompra?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0,
                    PagadoTotal = totalPagado,
                    Estado = compra.Estado,
                    MetodoPago = compra.MetodoPago ?? "EFECTIVO",
                    Observaciones = compra.Observaciones
                };

                var detalles = compra.DetallesCompra?.Select(d => new DetalleCompraProveedorDTO
                {
                    Id = d.Id,
                    IdCompra = d.IdCompra,
                    IdProducto = d.IdProducto,
                    DescripcionProducto = d.Producto?.Descripcion ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList() ?? new();

                // Generar PDF
                byte[] pdfBytes = _pdfService.GenerarComprobanteCompra(compraDTO, detalles);

                // Devolver archivo
                string nombreArchivo = $"Compra_{id}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar comprobante: {ex.Message}");
            }
        }
    }
}