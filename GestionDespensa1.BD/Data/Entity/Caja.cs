using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class Caja : EntityBase
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "ID Usuario")]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El importe de inicio es obligatorio.")]
        [Display(Name = "Importe Inicio")]
        [Precision(18, 2)]
        public decimal ImporteInicio { get; set; }

        // NUEVOS CAMPOS PARA CIERRE
        [Display(Name = "Importe Cierre")]
        [Precision(18, 2)]
        public decimal? ImporteCierre { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Abierta";

        [Display(Name = "Observaciones")]
        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string? Observaciones { get; set; }

        // Navigation properties
        public List<DetalleCaja> DetallesCaja { get; set; } = new List<DetalleCaja>();
    }
}