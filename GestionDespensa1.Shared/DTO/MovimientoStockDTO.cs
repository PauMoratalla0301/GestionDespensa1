using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.Shared.DTO
{
    public class MovimientoStockDTO
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string? Referencia { get; set; }
        public string? Observaciones { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
    }
}