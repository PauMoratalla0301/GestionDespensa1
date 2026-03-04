using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IDetalleCajaRepositorio : IRepositorio<DetalleCaja>
    {
        Task<List<DetalleCaja>> GetByCaja(int idCaja);
        Task<List<DetalleCaja>> GetByFecha(DateTime fecha);
        Task<List<DetalleCaja>> GetByTipo(string tipo);
        Task<decimal> GetTotalIngresosPorCaja(int idCaja);
        Task<decimal> GetTotalEgresosPorCaja(int idCaja);
        Task<List<DetalleCaja>> SelectWithRelations();
        Task<DetalleCaja?> SelectByIdWithRelations(int id);
    }
}