using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CajaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El importe de inicio es obligatorio.")]
        public decimal ImporteInicio { get; set; }

        // NUEVOS CAMPOS
        public decimal? ImporteCierre { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; } = "Abierta";

        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string? Observaciones { get; set; }
    }
}