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

        public async Task<List<DetalleCaja>> GetByCaja(int cajaId)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.IdCaja == cajaId)
                .ToListAsync();
        }

        public async Task<List<DetalleCaja>> GetByVenta(string idVenta)
        {
            return await _context.DetallesCaja
                .Include(d => d.Caja)
                .Where(d => d.IdVenta == idVenta)
                .ToListAsync();
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

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.DetallesCaja.AnyAsync(d => d.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(DetalleCaja detalle)
        {
            try
            {
                _context.DetallesCaja.Add(detalle);
                await _context.SaveChangesAsync();
                return detalle.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, DetalleCaja detalle)
        {
            try
            {
                var detalleExistente = await _context.DetallesCaja.FindAsync(id);
                if (detalleExistente == null)
                    return false;

                // Actualizar propiedades
                detalleExistente.IdVenta = detalle.IdVenta;
                detalleExistente.Monto = detalle.Monto;
                detalleExistente.IdCaja = detalle.IdCaja;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var detalle = await _context.DetallesCaja.FindAsync(id);
                if (detalle == null)
                    return false;

                _context.DetallesCaja.Remove(detalle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}