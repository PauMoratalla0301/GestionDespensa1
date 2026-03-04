using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IDashboardService
    {
        Task<HttpRespuesta<DashboardDTO>> Get();
    }
}