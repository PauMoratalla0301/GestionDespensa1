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
    public class CajasController : ControllerBase
    {
        private readonly ICajaRepositorio _repositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public CajasController(
            ICajaRepositorio repositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CajaDTO>>> Get()
        {
            try
            {
                var cajas = await _repositorio.SelectWithRelations();
                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);
                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CajaDTO>> Get(int id)
        {
            try
            {
                var caja = await _repositorio.SelectByIdWithRelations(id);
                if (caja == null)
                    return NotFound();

                var cajaDTO = _mapper.Map<CajaDTO>(caja);
                return Ok(cajaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByUsuario/{idUsuario}")]
        public async Task<ActionResult<List<CajaDTO>>> GetByUsuario(int idUsuario)
        {
            try
            {
                var cajas = await _repositorio.GetByUsuario(idUsuario.ToString());
                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);
                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<CajaDTO>>> GetByFecha(DateTime fecha)
        {
            try
            {
                var cajas = await _repositorio.GetByFecha(fecha);
                var cajasDTO = _mapper.Map<List<CajaDTO>>(cajas);
                return Ok(cajasDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("HayCajaAbiertaHoy")]
        public async Task<ActionResult<bool>> HayCajaAbiertaHoy()
        {
            try
            {
                var cajaHoy = await _context.Cajas
                    .AnyAsync(c => c.Fecha.Date == DateTime.Today.Date && c.Estado == "Abierta");

                return Ok(cajaHoy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCajaDTO crearCajaDTO)
        {
            try
            {
                // Validar importe inicial
                if (crearCajaDTO.ImporteInicio < 0)
                {
                    return BadRequest("❌ El importe inicial no puede ser negativo.");
                }

                // Validar que no exista otra caja abierta para hoy
                var cajaExistente = await _context.Cajas
                    .FirstOrDefaultAsync(c => c.Fecha.Date == DateTime.Today.Date && c.Estado == "Abierta");

                if (cajaExistente != null)
                {
                    return BadRequest("❌ Ya existe una caja abierta para hoy. No se puede abrir otra.");
                }

                // Validar que el usuario existe
                var usuario = await _usuarioRepositorio.SelectById(crearCajaDTO.IdUsuario);
                if (usuario == null)
                {
                    return BadRequest($"El usuario con ID {crearCajaDTO.IdUsuario} no existe");
                }

                if (usuario.Estado != "ACTIVO")
                {
                    return BadRequest("El usuario no está activo");
                }

                var caja = _mapper.Map<Caja>(crearCajaDTO);
                caja.Estado = "Abierta";

                var idCreado = await _repositorio.Insert(caja);

                if (idCreado <= 0)
                {
                    return BadRequest("No se pudo crear la caja");
                }

                return Ok(idCreado);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CajaDTO cajaDTO)
        {
            try
            {
                if (id != cajaDTO.Id)
                    return BadRequest("IDs no coinciden");

                var cajaExistente = await _context.Cajas.FindAsync(id);
                if (cajaExistente == null)
                    return NotFound();

                // Validar que no se pueda modificar una caja cerrada
                if (cajaExistente.Estado == "Cerrada")
                {
                    return BadRequest("❌ No se puede modificar una caja que ya está cerrada.");
                }

                // Si está cerrando la caja
                if (cajaDTO.Estado == "Cerrada" && cajaExistente.Estado == "Abierta")
                {
                    // Validar importe de cierre
                    if (!cajaDTO.ImporteCierre.HasValue || cajaDTO.ImporteCierre <= 0)
                    {
                        return BadRequest("❌ Debe ingresar un importe de cierre válido.");
                    }

                    // Verificar si hay ventas pendientes del día
                    var ventasPendientes = await _context.Ventas
                        .AnyAsync(v => v.FechaVenta.Date == DateTime.Today.Date
                                    && v.Estado != "Pagado"
                                    && v.Estado != "Cancelado");

                    if (ventasPendientes)
                    {
                        return BadRequest("❌ No se puede cerrar la caja porque hay ventas pendientes de pago.");
                    }

                    // Validar que el usuario que cierra sea el mismo que abrió
                    var usuarioActual = await _usuarioRepositorio.SelectById(cajaDTO.IdUsuario);
                    if (cajaExistente.IdUsuario != usuarioActual?.Id)  // 👈 CORREGIDO
                    {
                        return BadRequest("❌ Solo el usuario que abrió la caja puede cerrarla.");
                    }
                }

                var caja = _mapper.Map<Caja>(cajaDTO);
                var resultado = await _repositorio.Update(id, caja);

                if (!resultado)
                    return BadRequest("No se pudo actualizar la caja");

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // Validar que la caja no tenga movimientos
                var tieneMovimientos = await _context.DetallesCaja
                    .AnyAsync(d => d.IdCaja == id);

                if (tieneMovimientos)
                {
                    return BadRequest("❌ No se puede eliminar una caja que tiene movimientos asociados.");
                }

                var resultado = await _repositorio.Delete(id);
                if (!resultado)
                    return BadRequest("No se pudo eliminar la caja");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
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
                return count;
            }
            catch (Exception ex)
            {
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                return existe;
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}