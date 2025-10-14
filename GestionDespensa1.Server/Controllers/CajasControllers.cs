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
    [Route("api/Cajas")]
    public class CajasControllers : ControllerBase
    {
        private readonly ICajaRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public CajasControllers(ICajaRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CajaDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/Cajas");

                var cajas = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Cajas encontradas: {cajas?.Count ?? 0}");

                if (cajas == null || !cajas.Any())
                {
                    Console.WriteLine("ℹ️  No hay cajas, retornando lista vacía");
                    return new List<CajaDTO>();
                }

                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {cajasDTO.Count}");

                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET Cajas: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar cajas: {ex.Message}");
            }
        }

        
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de Cajas funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Cajas.CountAsync();
                Console.WriteLine($"📊 Total cajas en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<CajaDTO>>> GetSimple()
        {
            try
            {
                var cajas = await _context.Cajas
                    .Select(c => new CajaDTO
                    {
                        Id = c.Id,
                        IdUsuario = c.IdUsuario,
                        Fecha = c.Fecha,
                        ImporteInicio = c.ImporteInicio
                       
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Cajas: {cajas.Count}");
                return Ok(cajas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByUsuario/{idUsuario}")]
        public async Task<ActionResult<List<CajaDTO>>> GetByUsuario(string idUsuario)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando cajas por usuario: {idUsuario}");

                var cajas = await _repositorio.GetByUsuario(idUsuario);
                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);

                Console.WriteLine($"✅ Cajas encontradas para usuario {idUsuario}: {cajasDTO.Count}");
                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByUsuario {idUsuario}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<CajaDTO>>> GetByFecha(DateTime fecha)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando cajas por fecha: {fecha:yyyy-MM-dd}");

                var cajas = await _repositorio.GetByFecha(fecha);
                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);

                Console.WriteLine($"✅ Cajas encontradas para fecha {fecha:yyyy-MM-dd}: {cajasDTO.Count}");
                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByFecha {fecha:yyyy-MM-dd}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe caja {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCajaDTO crearCajaDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear caja para usuario: {crearCajaDTO.IdUsuario}");

                var caja = _mapper.Map<Caja>(crearCajaDTO);
                var idCreado = await _repositorio.Insert(caja);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear caja");
                    return BadRequest("No se pudo crear la caja");
                }

                Console.WriteLine($"✅ Caja creada con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST Caja: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CajaDTO cajaDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar caja {id}");

                if (id != cajaDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {cajaDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var caja = _mapper.Map<Caja>(cajaDTO);
                var resultado = await _repositorio.Update(id, caja);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar caja {id}");
                    return BadRequest("No se pudo actualizar la caja");
                }

                Console.WriteLine($"✅ Caja {id} actualizada correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT Caja {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar caja {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar caja {id}");
                    return BadRequest("La caja no se pudo borrar");
                }

                Console.WriteLine($"✅ Caja {id} eliminada correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE Caja {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}