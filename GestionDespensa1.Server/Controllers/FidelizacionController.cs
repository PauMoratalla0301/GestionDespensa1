using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.Server.Servicios;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FidelizacionController : ControllerBase
    {
        private readonly FidelizacionService _fidelizacionService;

        public FidelizacionController(FidelizacionService fidelizacionService)
        {
            _fidelizacionService = fidelizacionService;
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<List<ClienteFidelizacionDTO>>> GetClientesAnalisis()
        {
            try
            {
                var clientes = await _fidelizacionService.GetClientesConAnalisis();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("vip")]
        public async Task<ActionResult<List<ClienteFidelizacionDTO>>> GetClientesVIP()
        {
            try
            {
                var clientes = await _fidelizacionService.GetClientesVIP();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("sugerencias/{idCliente}")]
        public async Task<ActionResult<List<ProductoDTO>>> GetSugerencias(int idCliente)
        {
            try
            {
                var sugerencias = await _fidelizacionService.SugerirProductos(idCliente);
                return Ok(sugerencias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}