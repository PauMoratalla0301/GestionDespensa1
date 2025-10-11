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
    [Route("api/DetallesVenta")]
    public class DetallesVentaControllers : ControllerBase
    {
        private readonly IDetalleVentaRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public DetallesVentaControllers(IDetalleVentaRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DetalleVentaDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/DetallesVenta");

                var detalles = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Detalles de venta encontrados: {detalles?.Count ?? 0}");

                if (detalles == null || !detalles.Any())
                {
                    Console.WriteLine("ℹ️  No hay detalles de venta, retornando lista vacía");
                    return new List<DetalleVentaDTO>();
                }

                var detallesDTO = _mapper.Map<List<DetalleVentaDTO>>(detalles);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {detallesDTO.Count}");

                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET DetallesVenta: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar detalles de venta: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de DetallesVenta funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.DetallesVenta.CountAsync();
                Console.WriteLine($"📊 Total detalles de venta en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<DetalleVentaDTO>>> GetSimple()
        {
            try
            {
                var detalles = await _context.DetallesVenta
                    .Select(d => new DetalleVentaDTO
                    {
                        Id = d.Id,
                        IdVenta = d.IdVenta,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Detalles: {detalles.Count}");
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleVentaDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalle de venta ID: {id}");

                var detalle = await _repositorio.SelectByIdWithRelations(id);
                if (detalle == null)
                {
                    Console.WriteLine($"❌ Detalle de venta {id} no encontrado");
                    return NotFound();
                }

                var detalleDTO = _mapper.Map<DetalleVentaDTO>(detalle);
                Console.WriteLine($"✅ Detalle encontrado: ID {detalleDTO.Id}");
                return Ok(detalleDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByVenta/{ventaId:int}")]
        public async Task<ActionResult<List<DetalleVentaDTO>>> GetByVenta(int ventaId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por venta: {ventaId}");

                var detalles = await _repositorio.GetByVenta(ventaId);
                var detallesDTO = _mapper.Map<List<DetalleVentaDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para venta {ventaId}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByVenta {ventaId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProducto/{productoId:int}")]
        public async Task<ActionResult<List<DetalleVentaDTO>>> GetByProducto(int productoId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por producto: {productoId}");

                var detalles = await _repositorio.GetByProducto(productoId);
                var detallesDTO = _mapper.Map<List<DetalleVentaDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para producto {productoId}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByProducto {productoId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe detalle de venta {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearDetalleVentaDTO crearDetalleVentaDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear detalle de venta para venta: {crearDetalleVentaDTO.IdVenta}");

                var detalle = _mapper.Map<DetalleVenta>(crearDetalleVentaDTO);
                var idCreado = await _repositorio.Insert(detalle);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear detalle de venta");
                    return BadRequest("No se pudo crear el detalle de venta");
                }

                Console.WriteLine($"✅ Detalle de venta creado con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST DetalleVenta: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, DetalleVentaDTO detalleVentaDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar detalle de venta {id}");

                if (id != detalleVentaDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {detalleVentaDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var detalle = _mapper.Map<DetalleVenta>(detalleVentaDTO);
                var resultado = await _repositorio.Update(id, detalle);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar detalle de venta {id}");
                    return BadRequest("No se pudo actualizar el detalle de venta");
                }

                Console.WriteLine($"✅ Detalle de venta {id} actualizado correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT DetalleVenta {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar detalle de venta {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar detalle de venta {id}");
                    return BadRequest("El detalle de venta no se pudo borrar");
                }

                Console.WriteLine($"✅ Detalle de venta {id} eliminado correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE DetalleVenta {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}