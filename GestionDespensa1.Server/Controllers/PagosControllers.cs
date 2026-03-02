//using AutoMapper;
//using GestionDespensa1.BD.Data;
//using GestionDespensa1.BD.Data.Entity;
//using GestionDespensa1.Server.Repositorio;
//using GestionDespensa1.Shared.DTO;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace GestionDespensa1.Server.Controllers
//{
//    [ApiController]
//    [Route("api/Pagos")]


//    public class PagosController : ControllerBase  // ← Nombre corregido
//    {
//        private readonly IPagoRepositorio _repositorio;
//        private readonly IMapper _mapper;
//        private readonly Context _context;

//        public PagosController(ICajaRepositorio repositorio, IMapper mapper, Context context)  // ← Nombre corregido
//        {
//            _repositorio = repositorio;
//            _mapper = mapper;
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<List<PagoDTO>>> Get()
//        {
//            try
//            {

//                var pagos = await _repositorio.SelectWithRelations();

//                if (pagos == null || !pagos.Any())
//                {
//                    Console.WriteLine("ℹ️  No hay pagos, retornando lista vacía");
//                    return new List<PagoDTO>();
//                }

//                var pagosDTO = _mapper.Map<List<PagoDTO>>(pagos);
//                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {pagosDTO.Count}");

//                return Ok(pagosDTO);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error al cargar pagos: {ex.Message}");
//            }
//        }

//        // ✅ MÉTODO NUEVO - Para obtener pago por ID
//        [HttpGet("{id:int}")]
//        public async Task<ActionResult<PagoDTO>> Get(int id)
//        {
//            try
//            {

//                var pago = await _repositorio.GetById(id);
//                if (pago == null)

//                    return NotFound();


//                var pagoDTO = _mapper.Map<PagoDTO>(pago);
//                Console.WriteLine($"✅ Pago {id} encontrado");
//                return Ok(pagoDTO);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error en GET Pago {id}: {ex.Message}");
//                return StatusCode(500, $"Error al cargar pago: {ex.Message}");
//            }
//        }

//        [HttpGet("GetPendientes")]
//        public async Task<ActionResult<List<PagoDTO>>> GetPendientes()
//        {
//            try
//            {
//                var pagos = await _context.Pagos
//                    .Where(p => p.SaldoPendiente > 0)
//                    .ToListAsync();

//                var pagosDTO = _mapper.Map<List<PagoDTO>>(pagos);
//                return Ok(pagosDTO);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error: {ex.Message}");
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult<int>> Post(CrearPagoDTO crearPagoDTO)
//        {
//            try
//            {
//                var pago = _mapper.Map<Pago>(crearPagoDTO);
//                var idCreado = await _repositorio.Insert(pago);

//                pago.SaldoPendiente = pago.MontoTotal - pago.MontoPagado;

//                var idCreado = await _repositorio.Insert(pago);
//                if (idCreado == -1)
//                    return BadRequest("No se pudo crear el pago");

//                return Ok(idCreado);
//            }
//            catch (Exception err)
//            {
//                return BadRequest(err.Message);
//            }
//        }

//        [HttpPut("{id:int}")]
//        public async Task<ActionResult> Put(int id, PagoDTO pagoDTO)
//        {
//            try
//            {
//                Console.WriteLine($"✏️ Intentando actualizar pago {id}");

//                if (id != pagoDTO.Id)

//                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {pagoDTO.Id}");
//                return BadRequest("Datos Incorrectos");


//                var pago = _mapper.Map<Pago>(pagoDTO);
//                pago.SaldoPendiente = pago.MontoTotal - pago.MontoPagado;

//                var resultado = await _repositorio.Update(id, pago);
//                if (!resultado)
//                    return BadRequest("No se pudo actualizar el pago");

//                return Ok();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine($"❌ Error en PUT Pago {id}: {e.Message}");
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpDelete("{id:int}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            try
//            {
//                Console.WriteLine($"🗑️ Intentando eliminar pago {id}");

//                var resp = await _repositorio.Delete(id);
//                if (!resp)

//                    Console.WriteLine($"❌ No se pudo borrar pago {id}");
//                return BadRequest("El pago no se pudo borrar");


//                Console.WriteLine($"✅ Pago {id} eliminado correctamente");
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error en DELETE Pago {id}: {ex.Message}");
//                return BadRequest($"Error: {ex.Message}");
//            }
//        }
//    }

//}
