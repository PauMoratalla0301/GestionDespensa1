using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface ICajaService
    {
        Task<HttpRespuesta<List<CajaDTO>>> Get();
        Task<HttpRespuesta<CajaDTO>> Get(int id);
        Task<HttpRespuesta<List<CajaDTO>>> GetByUsuario(string idUsuario);
        Task<HttpRespuesta<List<CajaDTO>>> GetByFecha(DateTime fecha);
        Task<HttpRespuesta<int>> Insert(CrearCajaDTO caja);
        Task<HttpRespuesta<object>> Update(int id, CajaDTO caja);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}