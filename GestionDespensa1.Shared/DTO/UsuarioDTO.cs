using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        [EmailAddress(ErrorMessage = "Email no válido.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Rol { get; set; } = "EMPLEADO";

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "ACTIVO";
    }
}
