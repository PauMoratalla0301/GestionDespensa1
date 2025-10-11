using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class DetalleCompraProveedorRepositorio : Repositorio<DetalleCompraProveedor>, IDetalleCompraProveedorRepositorio
    {
        private readonly Context _context;

        public DetalleCompraProveedorRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DetalleCompraProveedor>> GetByCompra(int compraId)
        {
            return await _context.DetallesCompraProveedor
                .Include(d => d.CompraProveedor)
                .Include(d => d.Producto)
                .Where(d => d.IdCompra == compraId)
                .ToListAsync();
        }

        public async Task<List<DetalleCompraProveedor>> GetByProducto(int productoId)
        {
            return await _context.DetallesCompraProveedor
                .Include(d => d.CompraProveedor)
                .Include(d => d.Producto)
                .Where(d => d.IdProducto == productoId)
                .ToListAsync();
        }

        public async Task<List<DetalleCompraProveedor>> SelectWithRelations()
        {
            return await _context.DetallesCompraProveedor
                .Include(d => d.CompraProveedor)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        public async Task<DetalleCompraProveedor?> SelectByIdWithRelations(int id)
        {
            return await _context.DetallesCompraProveedor
                .Include(d => d.CompraProveedor)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.DetallesCompraProveedor.AnyAsync(d => d.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(DetalleCompraProveedor detalle)
        {
            try
            {
                _context.DetallesCompraProveedor.Add(detalle);
                await _context.SaveChangesAsync();
                return detalle.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, DetalleCompraProveedor detalle)
        {
            try
            {
                var detalleExistente = await _context.DetallesCompraProveedor.FindAsync(id);
                if (detalleExistente == null)
                    return false;

                // Actualizar propiedades
                detalleExistente.IdCompra = detalle.IdCompra;
                detalleExistente.IdProducto = detalle.IdProducto;
                detalleExistente.Cantidad = detalle.Cantidad;
                detalleExistente.PrecioUnitario = detalle.PrecioUnitario;

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
                var detalle = await _context.DetallesCompraProveedor.FindAsync(id);
                if (detalle == null)
                    return false;

                _context.DetallesCompraProveedor.Remove(detalle);
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