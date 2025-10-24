using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GestionDespensa1.Server.Repositorio
{
    public class CajaRepositorio : ICajaRepositorio
    {
        private readonly Context _context;

        public CajaRepositorio(Context context)
        {
            _context = context;
        }

        // ✅ MÉTODO NUEVO - Para obtener caja por ID (sin relaciones)
        public async Task<Caja?> GetById(int id)
        {
            return await _context.Cajas
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Caja>> SelectWithRelations()
        {
            return await _context.Cajas
                .Include(c => c.DetallesCaja)
                .ToListAsync();
        }

        public async Task<Caja?> SelectByIdWithRelations(int id)
        {
            return await _context.Cajas
                .Include(c => c.DetallesCaja)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Caja>> GetByUsuario(string idUsuario)
        {
            return await _context.Cajas
                .Where(c => c.IdUsuario == idUsuario)
                .Include(c => c.DetallesCaja)
                .ToListAsync();
        }

        public async Task<List<Caja>> GetByFecha(DateTime fecha)
        {
            return await _context.Cajas
                .Where(c => c.Fecha.Date == fecha.Date)
                .Include(c => c.DetallesCaja)
                .ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Cajas.AnyAsync(c => c.Id == id);
            }
            catch
            {
                return false;
            }
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
                return -1; // Retorna -1 en caso de error
            }
        }

        public async Task<bool> Update(int id, Caja caja)
        {
            try
            {
                var cajaExistente = await _context.Cajas.FindAsync(id);
                if (cajaExistente == null)
                    return false;

                // Actualizar propiedades
                cajaExistente.IdUsuario = caja.IdUsuario;
                cajaExistente.Fecha = caja.Fecha;
                cajaExistente.ImporteInicio = caja.ImporteInicio;

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
    }
}