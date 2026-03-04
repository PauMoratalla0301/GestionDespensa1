using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Servicios
{
    public class FidelizacionService
    {
        private readonly Context _context;

        public FidelizacionService(Context context)
        {
            _context = context;
        }

        // Calcular categoría de cliente basado en compras
        public async Task<List<ClienteFidelizacionDTO>> GetClientesConAnalisis()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var resultado = new List<ClienteFidelizacionDTO>();

            foreach (var cliente in clientes)
            {
                var ventas = await _context.Ventas
                    .Where(v => v.IdCliente == cliente.Id && v.Estado == "Pagado")
                    .OrderByDescending(v => v.FechaVenta)
                    .ToListAsync();

                var totalCompras = ventas.Sum(v => v.Total);
                var cantidadCompras = ventas.Count;
                var ultimaCompra = ventas.FirstOrDefault()?.FechaVenta;

                // Calcular puntos (ej: 10 puntos por cada $100)
                int puntos = (int)(totalCompras / 10);

                // Determinar categoría
                string categoria = "Ocasional";
                if (cantidadCompras >= 20)
                    categoria = "VIP";
                else if (cantidadCompras >= 10)
                    categoria = "Frecuente";
                else if (cantidadCompras == 0)
                    categoria = "Nuevo";

                // Últimas 5 compras
                var ultimasCompras = ventas.Take(5).Select(v => new CompraResumenDTO
                {
                    IdVenta = v.Id,
                    Fecha = v.FechaVenta,
                    Total = v.Total,
                    PuntosGanados = (int)(v.Total / 10)
                }).ToList();

                resultado.Add(new ClienteFidelizacionDTO
                {
                    IdCliente = cliente.Id,
                    NombreCliente = $"{cliente.Nombre} {cliente.Apellido}",
                    Email = cliente.Email ?? "",
                    Telefono = cliente.Telefono ?? "",
                    FechaNacimiento = null, // Podrías agregar campo a Cliente
                    PuntosAcumulados = puntos,
                    TotalCompras = totalCompras,
                    CantidadCompras = cantidadCompras,
                    UltimaCompra = ultimaCompra ?? DateTime.MinValue,
                    Categoria = categoria,
                    UltimasCompras = ultimasCompras
                });
            }

            return resultado.OrderByDescending(c => c.TotalCompras).ToList();
        }

        // Obtener clientes VIP (top 10%)
        public async Task<List<ClienteFidelizacionDTO>> GetClientesVIP()
        {
            var todos = await GetClientesConAnalisis();
            int countVIP = Math.Max(1, (int)(todos.Count * 0.1));
            return todos.Take(countVIP).ToList();
        }

        // Obtener clientes que cumplen años este mes
        public async Task<List<ClienteFidelizacionDTO>> GetClientesCumpleaniosMes()
        {
            // Implementar cuando agreguemos FechaNacimiento a Cliente
            return new List<ClienteFidelizacionDTO>();
        }

        // Sugerir productos basado en compras anteriores
        public async Task<List<ProductoDTO>> SugerirProductos(int idCliente)
        {
            var ventas = await _context.Ventas
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .Where(v => v.IdCliente == idCliente && v.Estado == "Pagado")
                .ToListAsync();

            // Obtener categorías más compradas
            var categorias = ventas
                .SelectMany(v => v.DetallesVenta)
                .GroupBy(d => d.Producto?.IdCategoria)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            // Sugerir productos de esas categorías que no haya comprado
            var productosComprados = ventas
                .SelectMany(v => v.DetallesVenta)
                .Select(d => d.IdProducto)
                .Distinct()
                .ToList();

            var sugerencias = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => categorias.Contains(p.IdCategoria) && !productosComprados.Contains(p.Id))
                .Take(5)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Descripcion = p.Descripcion,
                    PrecioUnitario = p.PrecioUnitario,
                    GananciaPorcentaje = p.GananciaPorcentaje,
                    StockActual = p.StockActual,
                    IdCategoria = p.IdCategoria,
                    Categoria = p.Categoria != null ? new CategoriaDTO
                    {
                        Id = p.Categoria.Id,
                        NombreCategoria = p.Categoria.NombreCategoria
                    } : null
                })
                .ToListAsync();

            return sugerencias;
        }
    }
}