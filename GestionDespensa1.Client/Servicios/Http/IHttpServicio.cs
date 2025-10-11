namespace GestionDespensa1.Client.Servicios.Http
{
    public interface IHttpServicio
    {
        Task<HttpRespuesta<T>> Get<T>(string url);
        Task<HttpRespuesta<object>> Post<T>(string url, T enviar);
        Task<HttpRespuesta<object>> Put<T>(string url, T enviar);
        Task<HttpRespuesta<object>> Delete(string url);
    }
}