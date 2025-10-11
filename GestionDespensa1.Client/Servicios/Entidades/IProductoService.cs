using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IProductoService
    {
        Task<HttpRespuesta<List<ProductoDTO>>> Get();
        Task<HttpRespuesta<ProductoDTO>> Get(int id);
        Task<HttpRespuesta<ProductoDTO>> GetByDescripcion(string descripcion);
        Task<HttpRespuesta<List<ProductoDTO>>> GetByCategoria(int categoriaId);
        Task<HttpRespuesta<List<ProductoDTO>>> GetStockBajo();
        Task<HttpRespuesta<int>> Insert(CrearProductoDTO producto);
        Task<HttpRespuesta<object>> Update(int id, ProductoDTO producto);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}