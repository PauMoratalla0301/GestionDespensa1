using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class NotificacionesService : INotificacionesService
    {
        private readonly IHttpServicio _httpServicio;

        public NotificacionesService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<NotificacionesDTO>> Get()
        {
            return await _httpServicio.Get<NotificacionesDTO>("api/Notificaciones");
        }
    }
}