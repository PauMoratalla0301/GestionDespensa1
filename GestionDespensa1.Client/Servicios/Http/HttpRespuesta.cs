namespace GestionDespensa1.Client.Servicios.Http
{
    public class HttpRespuesta<T>
    {
        public T? Respuesta { get; set; }
        public bool Error { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; } = null!;

        public HttpRespuesta(T? respuesta, bool error, HttpResponseMessage httpResponseMessage)
        {
            Respuesta = respuesta;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string> ObtenerError()
        {
            if (!Error) return string.Empty;

            var codigoEstatus = HttpResponseMessage.StatusCode;
            return codigoEstatus switch
            {
                System.Net.HttpStatusCode.NotFound => "Recurso no encontrado",
                System.Net.HttpStatusCode.BadRequest => await HttpResponseMessage.Content.ReadAsStringAsync(),
                System.Net.HttpStatusCode.Unauthorized => "No tienes autorización para este recurso",
                System.Net.HttpStatusCode.Forbidden => "No tienes permisos para este recurso",
                _ => "Ha ocurrido un error inesperado"
            };
        }
    }
}
