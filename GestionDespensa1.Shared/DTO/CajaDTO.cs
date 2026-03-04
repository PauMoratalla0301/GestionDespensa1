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
        public int IdUsuario { get; set; }  // 👈 CAMBIADO DE STRING A INT

        public string? NombreUsuario { get; set; }  // Para mostrar el nombre

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El importe de inicio es obligatorio.")]
        public decimal ImporteInicio { get; set; }

        public decimal? ImporteCierre { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; } = "Abierta";

        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string? Observaciones { get; set; }
    }
}