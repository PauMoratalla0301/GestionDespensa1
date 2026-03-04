using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.Server.Servicios;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ReportesService _reportesService;

        public ReportesController(ReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("ventas")]
        public async Task<ActionResult<ReporteVentasDTO>> GetVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = await _reportesService.GetVentasPorPeriodo(fechaInicio, fechaFin);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("productos-mas-vendidos")]
        public async Task<ActionResult<ReporteProductosDTO>> GetProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = await _reportesService.GetProductosMasVendidos(fechaInicio, fechaFin);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("clientes-deudores")]
        public async Task<ActionResult<ReporteDeudoresDTO>> GetClientesDeudores()
        {
            try
            {
                var reporte = await _reportesService.GetClientesDeudores();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo")]
        public async Task<ActionResult<ReporteStockBajoDTO>> GetStockBajo()
        {
            try
            {
                var reporte = await _reportesService.GetStockBajo();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}