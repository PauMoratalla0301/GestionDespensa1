using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class MovimientoStockRepositorio : Repositorio<MovimientoStock>, IMovimientoStockRepositorio
    {
        private readonly Context _context;

        public MovimientoStockRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MovimientoStock>> GetByProducto(int idProducto)
        {
            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.IdProducto == idProducto)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

        public async Task<List<MovimientoStock>> GetByFecha(DateTime fecha)
        {
            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.Fecha.Date == fecha.Date)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

        public async Task<List<MovimientoStock>> GetByTipo(string tipo)
        {
            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.Tipo == tipo)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }
    }
}