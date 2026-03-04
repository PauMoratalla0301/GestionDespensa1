using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.Server.Servicios;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportarController : ControllerBase
    {
        private readonly ReportesService _reportesService;
        private readonly ExcelService _excelService;

        public ExportarController(ReportesService reportesService, ExcelService excelService)
        {
            _reportesService = reportesService;
            _excelService = excelService;
        }

        [HttpGet("ventas/excel")]
        public async Task<IActionResult> ExportarVentasExcel(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = await _reportesService.GetVentasPorPeriodo(fechaInicio, fechaFin);
                var excelBytes = _excelService.GenerarExcelVentas(reporte);

                string nombreArchivo = $"Ventas_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("productos/excel")]
        public async Task<IActionResult> ExportarProductosExcel(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = await _reportesService.GetProductosMasVendidos(fechaInicio, fechaFin);
                var excelBytes = _excelService.GenerarExcelProductos(reporte);

                string nombreArchivo = $"Productos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("deudores/excel")]
        public async Task<IActionResult> ExportarDeudoresExcel()
        {
            try
            {
                var reporte = await _reportesService.GetClientesDeudores();
                var excelBytes = _excelService.GenerarExcelDeudores(reporte);

                string nombreArchivo = $"Deudores_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("stock-bajo/excel")]
        public async Task<IActionResult> ExportarStockBajoExcel()
        {
            try
            {
                var reporte = await _reportesService.GetStockBajo();
                var excelBytes = _excelService.GenerarExcelStockBajo(reporte);

                string nombreArchivo = $"StockBajo_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}