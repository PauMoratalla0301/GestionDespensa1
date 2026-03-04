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
    public class MovimientoStockController : ControllerBase
    {
        private readonly IMovimientoStockRepositorio _repositorio;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public MovimientoStockController(
            IMovimientoStockRepositorio repositorio,
            IProductoRepositorio productoRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovimientoStockDTO>>> Get()
        {
            try
            {
                var movimientos = await _context.MovimientosStock
                    .Include(m => m.Producto)
                    .OrderByDescending(m => m.Fecha)
                    .ToListAsync();

                var movimientosDTO = _mapper.Map<List<MovimientoStockDTO>>(movimientos);
                return Ok(movimientosDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovimientoStockDTO>> Get(int id)
        {
            try
            {
                var movimiento = await _context.MovimientosStock
                    .Include(m => m.Producto)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (movimiento == null)
                    return NotFound();

                var movimientoDTO = _mapper.Map<MovimientoStockDTO>(movimiento);
                return Ok(movimientoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProducto/{idProducto:int}")]
        public async Task<ActionResult<List<MovimientoStockDTO>>> GetByProducto(int idProducto)
        {
            try
            {
                var movimientos = await _repositorio.GetByProducto(idProducto);
                var movimientosDTO = _mapper.Map<List<MovimientoStockDTO>>(movimientos);
                return Ok(movimientosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<MovimientoStockDTO>>> GetByFecha(DateTime fecha)
        {
            try
            {
                var movimientos = await _repositorio.GetByFecha(fecha);
                var movimientosDTO = _mapper.Map<List<MovimientoStockDTO>>(movimientos);
                return Ok(movimientosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByTipo/{tipo}")]
        public async Task<ActionResult<List<MovimientoStockDTO>>> GetByTipo(string tipo)
        {
            try
            {
                var movimientos = await _repositorio.GetByTipo(tipo);
                var movimientosDTO = _mapper.Map<List<MovimientoStockDTO>>(movimientos);
                return Ok(movimientosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("ajuste")]
        public async Task<ActionResult<int>> AjusteManual(CrearMovimientoStockDTO crearMovimientoDTO)
        {
            try
            {
                // Validar producto
                var producto = await _productoRepositorio.SelectByIdWithRelations(crearMovimientoDTO.IdProducto);
                if (producto == null)
                    return BadRequest("Producto no encontrado");

                // Validar tipo de ajuste
                if (crearMovimientoDTO.Tipo != "AJUSTE_SUMA" && crearMovimientoDTO.Tipo != "AJUSTE_RESTA")
                    return BadRequest("Tipo de ajuste no válido");

                int stockAnterior = producto.StockActual;
                int stockNuevo;

                // Aplicar ajuste
                if (crearMovimientoDTO.Tipo == "AJUSTE_SUMA")
                {
                    stockNuevo = producto.StockActual + crearMovimientoDTO.Cantidad;
                    producto.StockActual = stockNuevo;
                }
                else // AJUSTE_RESTA
                {
                    if (producto.StockActual < crearMovimientoDTO.Cantidad)
                        return BadRequest($"Stock insuficiente. Actual: {producto.StockActual}");

                    stockNuevo = producto.StockActual - crearMovimientoDTO.Cantidad;
                    producto.StockActual = stockNuevo;
                }

                await _productoRepositorio.Update(producto.Id, producto);

                // Registrar movimiento
                var movimiento = new MovimientoStock
                {
                    IdProducto = crearMovimientoDTO.IdProducto,
                    Tipo = "AJUSTE",
                    Cantidad = crearMovimientoDTO.Cantidad,
                    Fecha = DateTime.Now,
                    Referencia = "Ajuste manual",
                    Observaciones = crearMovimientoDTO.Observaciones ?? "Ajuste de stock",
                    StockAnterior = stockAnterior,
                    StockNuevo = stockNuevo
                };

                var idMovimiento = await _repositorio.Insert(movimiento);
                return Ok(idMovimiento);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}