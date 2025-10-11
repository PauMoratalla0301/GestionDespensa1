using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IProveedorService
    {
        Task<HttpRespuesta<List<ProveedorDTO>>> Get();
        Task<HttpRespuesta<ProveedorDTO>> Get(int id);
        Task<HttpRespuesta<ProveedorDTO>> GetByCuit(string cuit);
        Task<HttpRespuesta<List<ProveedorDTO>>> GetByEstado(string estado);
        Task<HttpRespuesta<int>> Insert(CrearProveedorDTO proveedor);
        Task<HttpRespuesta<object>> Update(int id, ProveedorDTO proveedor);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}