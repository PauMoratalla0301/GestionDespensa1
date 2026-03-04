namespace GestionDespensa1.Client.Servicios.Http
{
    public interface IHttpServicio
    {
        Task<HttpRespuesta<T>> Get<T>(string url);
        Task<HttpRespuesta<object>> Post<T>(string url, T enviar);
        Task<HttpRespuesta<object>> Put<T>(string url, T enviar);
        Task<HttpRespuesta<object>> Delete(string url);
        Task<HttpRespuesta<T2>> Post<T1, T2>(string v, T1 loginDTO);
        void SetAuthorizationHeader(string v);
    }
}