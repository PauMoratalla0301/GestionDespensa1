using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IDetalleCajaService
    {
        Task<HttpRespuesta<List<DetalleCajaDTO>>> Get();
        Task<HttpRespuesta<DetalleCajaDTO>> Get(int id);
        Task<HttpRespuesta<List<DetalleCajaDTO>>> GetByCaja(int cajaId);
        Task<HttpRespuesta<List<DetalleCajaDTO>>> GetByVenta(string idVenta);
        Task<HttpRespuesta<int>> Insert(CrearDetalleCajaDTO detalleCaja);
        Task<HttpRespuesta<object>> Update(int id, DetalleCajaDTO detalleCaja);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}