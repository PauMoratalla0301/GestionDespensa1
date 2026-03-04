using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpServicio _httpServicio;

        public DashboardService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<DashboardDTO>> Get()
        {
            return await _httpServicio.Get<DashboardDTO>("api/Dashboard");
        }
    }
}