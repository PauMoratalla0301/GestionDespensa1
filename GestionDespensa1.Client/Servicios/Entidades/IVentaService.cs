using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IVentaService
    {
        Task<HttpRespuesta<List<VentaDTO>>> Get();
        Task<HttpRespuesta<VentaDTO>> Get(int id);
        Task<HttpRespuesta<List<VentaDTO>>> GetByCliente(int clienteId);
        Task<HttpRespuesta<List<VentaDTO>>> GetByFecha(DateTime fecha);
        Task<HttpRespuesta<List<VentaDTO>>> GetConSaldoPendiente();
        Task<HttpRespuesta<int>> Insert(CrearVentaDTO venta);
        Task<HttpRespuesta<object>> Update(int id, VentaDTO venta);
        Task<HttpRespuesta<object>> Delete(int id);
        Task<HttpRespuesta<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha); // Cambiado a HttpRespuesta
    }
}