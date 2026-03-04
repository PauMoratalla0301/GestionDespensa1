using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IMovimientoStockRepositorio : IRepositorio<MovimientoStock>
    {
        Task<List<MovimientoStock>> GetByProducto(int idProducto);
        Task<List<MovimientoStock>> GetByFecha(DateTime fecha);
        Task<List<MovimientoStock>> GetByTipo(string tipo);
    }
}