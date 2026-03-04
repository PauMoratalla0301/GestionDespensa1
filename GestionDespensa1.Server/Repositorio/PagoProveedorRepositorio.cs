using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class PagoProveedorRepositorio : Repositorio<PagoProveedor>, IPagoProveedorRepositorio
    {
        private readonly Context _context;

        public PagoProveedorRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PagoProveedor>> GetByCompra(int idCompra)
        {
            return await _context.PagosProveedor
                .Include(p => p.Compra)
                    .ThenInclude(c => c.Proveedor)
                .Where(p => p.IdCompra == idCompra)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<List<PagoProveedor>> GetByProveedor(int idProveedor)
        {
            return await _context.PagosProveedor
                .Include(p => p.Compra)
                .Where(p => p.Compra.IdProveedor == idProveedor)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPagadoPorCompra(int idCompra)
        {
            return await _context.PagosProveedor
                .Where(p => p.IdCompra == idCompra)
                .SumAsync(p => p.Monto);
        }
    }
}