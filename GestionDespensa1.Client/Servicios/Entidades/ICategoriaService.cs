using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface ICategoriaService
    {
        Task<HttpRespuesta<List<CategoriaDTO>>> Get();
        Task<HttpRespuesta<CategoriaDTO>> Get(int id);
        Task<HttpRespuesta<CategoriaDTO>> GetByNombre(string nombre);
        Task<HttpRespuesta<int>> Insert(CrearCategoriaDTO categoria);
        Task<HttpRespuesta<object>> Update(int id, CategoriaDTO categoria);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}