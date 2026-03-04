using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.Server.Servicios;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly NotificacionesService _notificacionesService;

        public NotificacionesController(NotificacionesService notificacionesService)
        {
            _notificacionesService = notificacionesService;
        }

        [HttpGet]
        public async Task<ActionResult<NotificacionesDTO>> Get()
        {
            try
            {
                var notificaciones = await _notificacionesService.ObtenerNotificaciones();
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}