using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IClienteService
    {
        Task<HttpRespuesta<List<ClienteDTO>>> Get();
        Task<HttpRespuesta<ClienteDTO>> Get(int id);
        Task<HttpRespuesta<ClienteDTO>> GetByDni(string dni);
        Task<HttpRespuesta<List<ClienteDTO>>> GetConSaldoPendiente();
        Task<HttpRespuesta<int>> Insert(CrearClienteDTO cliente);
        Task<HttpRespuesta<object>> Update(int id, ClienteDTO cliente);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}