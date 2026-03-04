using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.Shared.DTO
{
    public class PagoProveedorDTO
    {
        public int Id { get; set; }
        public int IdCompra { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string MedioPago { get; set; } = "Efectivo";
        public string? Observaciones { get; set; }
        public string? Proveedor { get; set; }
        public int? IdProveedor { get; set; }
        public decimal? TotalCompra { get; set; }
        public decimal? PagadoTotal { get; set; }
    }

    public class CrearPagoProveedorDTO
    {
        [Required]
        public int IdCompra { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string MedioPago { get; set; } = "Efectivo";

        [MaxLength(500)]
        public string? Observaciones { get; set; }
    }
}