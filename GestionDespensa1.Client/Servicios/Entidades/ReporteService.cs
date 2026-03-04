using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class ReportesService : IReportesService
    {
        private readonly IHttpServicio _httpServicio;

        public ReportesService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<ReporteVentasDTO>> GetVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _httpServicio.Get<ReporteVentasDTO>(
                $"api/Reportes/ventas?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<ReporteProductosDTO>> GetProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _httpServicio.Get<ReporteProductosDTO>(
                $"api/Reportes/productos-mas-vendidos?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<ReporteDeudoresDTO>> GetClientesDeudores()
        {
            return await _httpServicio.Get<ReporteDeudoresDTO>("api/Reportes/clientes-deudores");
        }

        public async Task<HttpRespuesta<ReporteStockBajoDTO>> GetStockBajo()
        {
            return await _httpServicio.Get<ReporteStockBajoDTO>("api/Reportes/stock-bajo");
        }
    }
}