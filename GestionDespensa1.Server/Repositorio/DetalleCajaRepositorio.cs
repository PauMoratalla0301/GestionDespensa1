using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class DetalleCajaRepositorio : Repositorio<DetalleCaja>, IDetalleCajaRepositorio
    {
        private readonly Context _context;

        public DetalleCajaRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DetalleCaja>> GetByCaja(int idCaja)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.IdCaja == idCaja)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();
        }

        public async Task<List<DetalleCaja>> GetByFecha(DateTime fecha)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.Fecha.Date == fecha.Date)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();
        }

        public async Task<List<DetalleCaja>> GetByTipo(string tipo)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.Tipo == tipo)
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalIngresosPorCaja(int idCaja)
        {
            return await _context.DetallesCaja
                .Where(d => d.IdCaja == idCaja && d.Tipo == "INGRESO")
                .SumAsync(d => d.Monto);
        }

        public async Task<decimal> GetTotalEgresosPorCaja(int idCaja)
        {
            return await _context.DetallesCaja
                .Where(d => d.IdCaja == idCaja && d.Tipo == "EGRESO")
                .SumAsync(d => d.Monto);
        }

        public async Task<List<DetalleCaja>> SelectWithRelations()
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .ToListAsync();
        }

        public async Task<DetalleCaja?> SelectByIdWithRelations(int id)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}