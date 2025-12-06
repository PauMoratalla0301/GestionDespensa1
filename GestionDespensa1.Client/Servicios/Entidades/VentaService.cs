using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class VentaService : IVentaService
    {
        private readonly IHttpServicio _httpServicio;

        public VentaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> Get()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas");
        }

        public async Task<HttpRespuesta<VentaDTO>> Get(int id)
        {
            return await _httpServicio.Get<VentaDTO>($"api/Ventas/{id}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetByCliente(int clienteId)
        {
            return await _httpServicio.Get<List<VentaDTO>>($"api/Ventas/GetByCliente/{clienteId}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetByFecha(DateTime fecha)
        {
            return await _httpServicio.Get<List<VentaDTO>>($"api/Ventas/GetByFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetConSaldoPendiente()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas/GetConSaldoPendiente");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearVentaDTO venta)
        {
            var respuesta = await _httpServicio.Post("api/Ventas", venta);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, VentaDTO venta)
        {
            return await _httpServicio.Put($"api/Ventas/{id}", venta);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Ventas/{id}");
        }

        public async Task<HttpRespuesta<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha)
        {
            return await _httpServicio.Get<ResumenVentasDTO>($"api/Ventas/GetResumenPorFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<bool>> SaldarVentaSimple(int id)
        {
            try
            {
                // Crear un objeto MINIMAL que seguro funcione
                var datos = new
                {
                    Estado = "Pagado",
                    MontoPagado = 0, // El backend calculará el total
                    SaldoPendiente = 0
                };

                // Usar PATCH o PUT según lo que acepte tu backend
                var respuesta = await _httpServicio.Put($"api/Ventas/{id}/saldar", datos);

                if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
                {
                    return new HttpRespuesta<bool>(true, false, respuesta.HttpResponseMessage);
                }
                else
                {
                    // Si falla, intentar con PUT normal
                    var ventaExistente = await Get(id);
                    if (!ventaExistente.Error && ventaExistente.Respuesta != null)
                    {
                        var venta = ventaExistente.Respuesta;
                        var ventaActualizada = new VentaDTO
                        {
                            Id = venta.Id,
                            IdCliente = venta.IdCliente,
                            FechaVenta = venta.FechaVenta,
                            Estado = "Pagado",
                            Total = venta.Total,
                            MontoPagado = venta.Total,
                            SaldoPendiente = 0,
                            MetodoPago = venta.MetodoPago,
                            Notas = venta.Notas
                        };

                        var respuesta2 = await Update(id, ventaActualizada);
                        return new HttpRespuesta<bool>(
                            !respuesta2.Error,
                            respuesta2.Error,
                            respuesta2.HttpResponseMessage
                        );
                    }

                    return new HttpRespuesta<bool>(false, true, respuesta.HttpResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<bool>(false, true, null);
            }
        }
    }
}
