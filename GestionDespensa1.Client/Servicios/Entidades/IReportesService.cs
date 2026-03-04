using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IReportesService
    {
        Task<HttpRespuesta<ReporteVentasDTO>> GetVentasPorPeriodo(DateTime fechaInicio, DateTime fechaFin);
        Task<HttpRespuesta<ReporteProductosDTO>> GetProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin);
        Task<HttpRespuesta<ReporteDeudoresDTO>> GetClientesDeudores();
        Task<HttpRespuesta<ReporteStockBajoDTO>> GetStockBajo();
    }
}