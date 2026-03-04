using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Http
{
    public class HttpServicio : IHttpServicio
    {
        private readonly HttpClient _httpClient;

        public HttpServicio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthorizationHeader(string? token)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            if (!string.IsNullOrEmpty(token))
            {
                string authHeader = token.StartsWith("Bearer ") ? token : $"Bearer {token}";
                _httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
            }
        }

        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                ? new HttpRespuesta<T>(await response.Content.ReadFromJsonAsync<T>(), false, response)
                : new HttpRespuesta<T>(default, true, response);
        }

        public async Task<HttpRespuesta<object>> Post<T>(string url, T enviar)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, enviar);
                return new HttpRespuesta<object>(null, !response.IsSuccessStatusCode, response);
            }
            catch (Exception ex)
            {
                return CrearRespuestaError($"Error de conexión: {ex.Message}");
            }
        }

        public async Task<HttpRespuesta<T2>> Post<T1, T2>(string url, T1 enviar)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, enviar);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<T2>();
                    return new HttpRespuesta<T2>(resultado, false, response);
                }

                return new HttpRespuesta<T2>(default, true, response);
            }
            catch (Exception ex)
            {
                return new HttpRespuesta<T2>(default, true,
                    new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent($"Error de conexión: {ex.Message}")
                    });
            }
        }

        public async Task<HttpRespuesta<object>> Put<T>(string url, T enviar)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, enviar);
                return new HttpRespuesta<object>(null, !response.IsSuccessStatusCode, response);
            }
            catch (Exception ex)
            {
                return CrearRespuestaError($"Error de conexión: {ex.Message}");
            }
        }

        public async Task<HttpRespuesta<object>> Delete(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return new HttpRespuesta<object>(null, !response.IsSuccessStatusCode, response);
        }

        private static HttpRespuesta<object> CrearRespuestaError(string mensaje)
        {
            return new HttpRespuesta<object>(null, true,
                new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(mensaje)
                });
        }
    }
}