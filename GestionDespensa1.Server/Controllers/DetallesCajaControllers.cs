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
    [Route("api/DetallesCaja")]
    public class DetallesCajaControllers : ControllerBase
    {
        private readonly IDetalleCajaRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public DetallesCajaControllers(IDetalleCajaRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DetalleCajaDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/DetallesCaja");

                var detalles = await _repositorio.Select();
                Console.WriteLine($"✅ Detalles de caja encontrados: {detalles?.Count ?? 0}");

                if (detalles == null || !detalles.Any())
                {
                    Console.WriteLine("ℹ️  No hay detalles de caja, retornando lista vacía");
                    return new List<DetalleCajaDTO>();
                }

                var detallesDTO = _mapper.Map<List<DetalleCajaDTO>>(detalles);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {detallesDTO.Count}");

                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET DetallesCaja: {ex.Message}");
                return StatusCode(500, $"Error al cargar detalles de caja: {ex.Message}");
            }
        }

        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de DetallesCaja funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.DetallesCaja.CountAsync();
                Console.WriteLine($"📊 Total detalles de caja en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<DetalleCajaDTO>>> GetSimple()
        {
            try
            {
                var detalles = await _context.DetallesCaja
                    .Select(d => new DetalleCajaDTO
                    {
                        Id = d.Id,
                        IdCaja = d.IdCaja,
                        Tipo = d.Tipo,
                        Concepto = d.Concepto,
                        Monto = d.Monto,
                        Fecha = d.Fecha,
                        Referencia = d.Referencia
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
        public async Task<ActionResult<DetalleCajaDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalle de caja ID: {id}");

                // Cambiar SelectByIdWithRelations por SelectById, ya que no existe el primero en la interfaz
                var detalle = await _repositorio.SelectById(id);
                if (detalle == null)
                {
                    Console.WriteLine($"❌ Detalle de caja {id} no encontrado");
                    return NotFound();
                }

                var detalleDTO = _mapper.Map<DetalleCajaDTO>(detalle);
                Console.WriteLine($"✅ Detalle encontrado: ID {detalleDTO.Id}");
                return Ok(detalleDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCaja/{cajaId:int}")]
        public async Task<ActionResult<List<DetalleCajaDTO>>> GetByCaja(int cajaId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por caja: {cajaId}");

                var detalles = await _repositorio.GetByCaja(cajaId);
                var detallesDTO = _mapper.Map<List<DetalleCajaDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para caja {cajaId}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByCaja {cajaId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<DetalleCajaDTO>>> GetByFecha(DateTime fecha)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por fecha: {fecha:yyyy-MM-dd}");

                var detalles = await _repositorio.GetByFecha(fecha);
                var detallesDTO = _mapper.Map<List<DetalleCajaDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para fecha {fecha:yyyy-MM-dd}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByFecha {fecha:yyyy-MM-dd}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByTipo/{tipo}")]
        public async Task<ActionResult<List<DetalleCajaDTO>>> GetByTipo(string tipo)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por tipo: {tipo}");

                var detalles = await _repositorio.GetByTipo(tipo);
                var detallesDTO = _mapper.Map<List<DetalleCajaDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para tipo {tipo}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByTipo {tipo}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // 👇 ELIMINAR COMPLETAMENTE el método GetByVenta (no existe en la entidad)
        // [HttpGet("GetByVenta/{idVenta}")] ...

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe detalle de caja {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearDetalleCajaDTO crearDetalleCajaDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear detalle de caja para caja: {crearDetalleCajaDTO.IdCaja}");

                var detalle = _mapper.Map<DetalleCaja>(crearDetalleCajaDTO);
                var idCreado = await _repositorio.Insert(detalle);

                if (idCreado <= 0)
                {
                    Console.WriteLine($"❌ Error al crear detalle de caja");
                    return BadRequest("No se pudo crear el detalle de caja");
                }

                Console.WriteLine($"✅ Detalle de caja creado con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST DetalleCaja: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, DetalleCajaDTO detalleCajaDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar detalle de caja {id}");

                if (id != detalleCajaDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {detalleCajaDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var detalle = _mapper.Map<DetalleCaja>(detalleCajaDTO);
                var resultado = await _repositorio.Update(id, detalle);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar detalle de caja {id}");
                    return BadRequest("No se pudo actualizar el detalle de caja");
                }

                Console.WriteLine($"✅ Detalle de caja {id} actualizado correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT DetalleCaja {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar detalle de caja {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar detalle de caja {id}");
                    return BadRequest("El detalle de caja no se pudo borrar");
                }

                Console.WriteLine($"✅ Detalle de caja {id} eliminado correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE DetalleCaja {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}