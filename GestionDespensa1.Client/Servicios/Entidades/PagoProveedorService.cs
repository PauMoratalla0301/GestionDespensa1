using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class PagoProveedorService : IPagoProveedorService
    {
        private readonly IHttpServicio _httpServicio;

        public PagoProveedorService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<PagoProveedorDTO>>> GetByCompra(int idCompra)
        {
            return await _httpServicio.Get<List<PagoProveedorDTO>>($"api/PagosProveedor/GetByCompra/{idCompra}");
        }

        public async Task<HttpRespuesta<List<PagoProveedorDTO>>> GetByProveedor(int idProveedor)
        {
            return await _httpServicio.Get<List<PagoProveedorDTO>>($"api/PagosProveedor/GetByProveedor/{idProveedor}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearPagoProveedorDTO pago)
        {
            return await _httpServicio.Post<CrearPagoProveedorDTO, int>("api/PagosProveedor", pago);
        }
    }
}