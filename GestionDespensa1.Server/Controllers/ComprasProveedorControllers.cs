using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Server.Repositorio;
using AutoMapper;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasProveedorController : ControllerBase
    {
        private readonly ICompraProveedorRepositorio _repositorio;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IMovimientoStockRepositorio _movimientoStockRepositorio;
        private readonly IPagoProveedorRepositorio _pagoProveedorRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public ComprasProveedorController(
            ICompraProveedorRepositorio repositorio,
            IProductoRepositorio productoRepositorio,
            IMovimientoStockRepositorio movimientoStockRepositorio,
            IPagoProveedorRepositorio pagoProveedorRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _productoRepositorio = productoRepositorio;
            _movimientoStockRepositorio = movimientoStockRepositorio;
            _pagoProveedorRepositorio = pagoProveedorRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompraProveedorDTO>>> Get()
        {
            try
            {
                var compras = await _context.ComprasProveedor
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                    .OrderByDescending(c => c.FechaCompra)
                    .ToListAsync();

                var comprasDTO = new List<CompraProveedorDTO>();

                foreach (var c in compras)
                {
                    var totalPagado = await _pagoProveedorRepositorio.GetTotalPagadoPorCompra(c.Id);

                    comprasDTO.Add(new CompraProveedorDTO
                    {
                        Id = c.Id,
                        IdProveedor = c.IdProveedor,
                        NombreProveedor = c.Proveedor?.Nombre ?? "",
                        FechaCompra = c.FechaCompra,
                        Total = c.DetallesCompra?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0,
                        PagadoTotal = totalPagado,
                        Estado = c.Estado,
                        MetodoPago = c.MetodoPago ?? "EFECTIVO",
                        Observaciones = c.Observaciones,
                        DetallesCompra = c.DetallesCompra?.Select(d => new DetalleCompraProveedorDTO
                        {
                            Id = d.Id,
                            IdCompra = d.IdCompra,
                            IdProducto = d.IdProducto,
                            DescripcionProducto = d.Producto?.Descripcion ?? "",
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario
                        }).ToList() ?? new()
                    });
                }

                return Ok(comprasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompraProveedorDTO>> Get(int id)
        {
            try
            {
                var compra = await _context.ComprasProveedor
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compra == null)
                    return NotFound();

                var totalPagado = await _pagoProveedorRepositorio.GetTotalPagadoPorCompra(compra.Id);

                var compraDTO = new CompraProveedorDTO
                {
                    Id = compra.Id,
                    IdProveedor = compra.IdProveedor,
                    NombreProveedor = compra.Proveedor?.Nombre ?? "",
                    FechaCompra = compra.FechaCompra,
                    Total = compra.DetallesCompra?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0,
                    PagadoTotal = totalPagado,
                    Estado = compra.Estado,
                    MetodoPago = compra.MetodoPago ?? "EFECTIVO",
                    Observaciones = compra.Observaciones,
                    DetallesCompra = compra.DetallesCompra?.Select(d => new DetalleCompraProveedorDTO
                    {
                        Id = d.Id,
                        IdCompra = d.IdCompra,
                        IdProducto = d.IdProducto,
                        DescripcionProducto = d.Producto?.Descripcion ?? "",
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList() ?? new()
                };

                return Ok(compraDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProveedor/{idProveedor:int}")]
        public async Task<ActionResult<List<CompraProveedorDTO>>> GetByProveedor(int idProveedor)
        {
            try
            {
                var compras = await _context.ComprasProveedor
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                    .Where(c => c.IdProveedor == idProveedor)
                    .OrderByDescending(c => c.FechaCompra)
                    .ToListAsync();

                var comprasDTO = new List<CompraProveedorDTO>();

                foreach (var c in compras)
                {
                    var totalPagado = await _pagoProveedorRepositorio.GetTotalPagadoPorCompra(c.Id);

                    comprasDTO.Add(new CompraProveedorDTO
                    {
                        Id = c.Id,
                        IdProveedor = c.IdProveedor,
                        NombreProveedor = c.Proveedor?.Nombre ?? "",
                        FechaCompra = c.FechaCompra,
                        Total = c.DetallesCompra?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0,
                        PagadoTotal = totalPagado,
                        Estado = c.Estado,
                        MetodoPago = c.MetodoPago ?? "EFECTIVO",
                        Observaciones = c.Observaciones,
                        DetallesCompra = c.DetallesCompra?.Select(d => new DetalleCompraProveedorDTO
                        {
                            Id = d.Id,
                            IdCompra = d.IdCompra,
                            IdProducto = d.IdProducto,
                            DescripcionProducto = d.Producto?.Descripcion ?? "",
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario
                        }).ToList() ?? new()
                    });
                }

                return Ok(comprasDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCompraProveedorDTO crearCompraDTO)
        {
            try
            {
                // Validar proveedor
                var proveedor = await _context.Proveedores.FindAsync(crearCompraDTO.IdProveedor);
                if (proveedor == null)
                    return BadRequest("Proveedor no encontrado");

                // Buscar caja abierta del día
                var cajaHoy = await _context.Cajas
                    .FirstOrDefaultAsync(c => c.Fecha.Date == DateTime.Today.Date && c.Estado == "Abierta");

                // Crear compra
                var compra = new CompraProveedor
                {
                    IdProveedor = crearCompraDTO.IdProveedor,
                    FechaCompra = crearCompraDTO.FechaCompra,
                    Estado = crearCompraDTO.Estado,
                    MetodoPago = crearCompraDTO.MetodoPago,
                    Observaciones = crearCompraDTO.Observaciones
                };

                _context.ComprasProveedor.Add(compra);
                await _context.SaveChangesAsync();

                decimal totalCompra = 0;

                // Procesar detalles y actualizar stock
                foreach (var detalle in crearCompraDTO.DetallesCompra)
                {
                    // Validar producto
                    var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                    if (producto == null)
                        return BadRequest($"Producto ID {detalle.IdProducto} no encontrado");

                    // Insertar detalle
                    var detalleCompra = new DetalleCompraProveedor
                    {
                        IdCompra = compra.Id,
                        IdProducto = detalle.IdProducto,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario
                    };
                    _context.DetallesCompraProveedor.Add(detalleCompra);

                    totalCompra += detalle.Cantidad * detalle.PrecioUnitario;

                    int stockAnterior = producto.StockActual;
                    producto.StockActual += detalle.Cantidad;

                    // Registrar movimiento de stock
                    var movimiento = new MovimientoStock
                    {
                        IdProducto = detalle.IdProducto,
                        Tipo = "COMPRA",
                        Cantidad = detalle.Cantidad,
                        Fecha = DateTime.Now,
                        Referencia = $"Compra #{compra.Id}",
                        StockAnterior = stockAnterior,
                        StockNuevo = producto.StockActual
                    };
                    _context.MovimientosStock.Add(movimiento);
                }

                // Actualizar total de la compra
                compra.Total = totalCompra;

                // Si la compra está pagada, registrar pago automático
                if (crearCompraDTO.Estado == "PAGADA")
                {
                    var pago = new PagoProveedor
                    {
                        IdCompra = compra.Id,
                        Fecha = DateTime.Now,
                        Monto = totalCompra,
                        MedioPago = crearCompraDTO.MetodoPago ?? "EFECTIVO",
                        Observaciones = "Pago automático al crear compra"
                    };
                    _context.PagosProveedor.Add(pago);

                    // Registrar egreso en caja
                    if (cajaHoy != null)
                    {
                        var detalleCaja = new DetalleCaja
                        {
                            IdCaja = cajaHoy.Id,
                            Tipo = "EGRESO",
                            Concepto = $"Compra a {proveedor.Nombre}",
                            Monto = totalCompra,
                            Fecha = DateTime.Now,
                            Referencia = $"Compra #{compra.Id}"
                        };
                        _context.DetallesCaja.Add(detalleCaja);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(compra.Id);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CompraProveedorDTO compraDTO)
        {
            try
            {
                var compra = await _context.ComprasProveedor.FindAsync(id);
                if (compra == null)
                    return NotFound();

                compra.Estado = compraDTO.Estado;
                compra.MetodoPago = compraDTO.MetodoPago;
                compra.Observaciones = compraDTO.Observaciones;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var compra = await _context.ComprasProveedor
                    .Include(c => c.DetallesCompra)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compra == null)
                    return NotFound();

                // Eliminar pagos asociados
                var pagos = await _context.PagosProveedor.Where(p => p.IdCompra == id).ToListAsync();
                _context.PagosProveedor.RemoveRange(pagos);

                _context.DetallesCompraProveedor.RemoveRange(compra.DetallesCompra);
                _context.ComprasProveedor.Remove(compra);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}