using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class PagoVentaRepositorio : Repositorio<PagoVenta>, IPagoVentaRepositorio
    {
        private readonly Context _context;

        public PagoVentaRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PagoVenta>> GetByVenta(int idVenta)
        {
            return await _context.PagosVenta
                .Include(p => p.Venta)
                    .ThenInclude(v => v.Cliente)
                .Where(p => p.IdVenta == idVenta)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<List<PagoVenta>> GetByCliente(int idCliente)
        {
            return await _context.PagosVenta
                .Include(p => p.Venta)
                .Where(p => p.Venta.IdCliente == idCliente)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPagadoPorVenta(int idVenta)
        {
            return await _context.PagosVenta
                .Where(p => p.IdVenta == idVenta)
                .SumAsync(p => p.Monto);
        }
    }
}