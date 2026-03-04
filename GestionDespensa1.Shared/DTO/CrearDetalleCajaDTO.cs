using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearDetalleCajaDTO
    {
        [Required]
        public int IdCaja { get; set; }

        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = "INGRESO";

        [Required]
        [MaxLength(200)]
        public string Concepto { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Referencia { get; set; }
    }
}
