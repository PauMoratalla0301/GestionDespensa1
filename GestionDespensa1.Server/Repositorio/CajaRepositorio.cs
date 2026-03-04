using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class CajaRepositorio : ICajaRepositorio
    {
        private readonly Context _context;

        public CajaRepositorio(Context context)
        {
            _context = context;
        }

        public async Task<Caja?> GetById(int id)
        {
            return await _context.Cajas
                .Include(c => c.Usuario) // Incluir Usuario
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Caja>> SelectWithRelations()
        {
            return await _context.Cajas
                .Include(c => c.DetallesCaja)
                .Include(c => c.Usuario) // Incluir Usuario
                .ToListAsync();
        }

        public async Task<Caja?> SelectByIdWithRelations(int id)
        {
            return await _context.Cajas
                .Include(c => c.DetallesCaja)
                .Include(c => c.Usuario) // Incluir Usuario
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Caja>> GetByUsuario(int idUsuario) // Cambiado a int
        {
            return await _context.Cajas
                .Where(c => c.IdUsuario == idUsuario)
                .Include(c => c.DetallesCaja)
                .Include(c => c.Usuario)
                .ToListAsync();
        }

        public async Task<List<Caja>> GetByFecha(DateTime fecha)
        {
            return await _context.Cajas
                .Where(c => c.Fecha.Date == fecha.Date)
                .Include(c => c.DetallesCaja)
                .Include(c => c.Usuario)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Cajas.AnyAsync(c => c.Id == id);
        }

        public async Task<int> Insert(Caja caja)
        {
            try
            {
                _context.Cajas.Add(caja);
                await _context.SaveChangesAsync();
                return caja.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, Caja caja)
        {
            try
            {
                var cajaExistente = await _context.Cajas.FindAsync(id);
                if (cajaExistente == null)
                    return false;

                cajaExistente.IdUsuario = caja.IdUsuario;
                cajaExistente.Fecha = caja.Fecha;
                cajaExistente.ImporteInicio = caja.ImporteInicio;
                cajaExistente.ImporteCierre = caja.ImporteCierre;
                cajaExistente.Estado = caja.Estado;
                cajaExistente.Observaciones = caja.Observaciones;

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
                var caja = await _context.Cajas.FindAsync(id);
                if (caja == null)
                    return false;

                _context.Cajas.Remove(caja);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<List<Caja>> GetByUsuario(string idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}