namespace GestionDespensa1.Shared.DTO
{
    public class NotificacionesDTO
    {
        public int StockBajo { get; set; }
        public int ClientesDeudores { get; set; }
        public int ComprasPendientes { get; set; }
        public int VentasPendientes { get; set; }

        public bool HayNotificaciones =>
            StockBajo > 0 ||
            ClientesDeudores > 0 ||
            ComprasPendientes > 0 ||
            VentasPendientes > 0;
    }
}