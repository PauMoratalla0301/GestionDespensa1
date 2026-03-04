using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.Shared.DTO
{
    public class DetalleCajaDTO
    {
        public int Id { get; set; }
        public int IdCaja { get; set; }
        public string Tipo { get; set; } = "INGRESO";
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Referencia { get; set; }
        public string? NombreCaja { get; set; }
    }

   
}