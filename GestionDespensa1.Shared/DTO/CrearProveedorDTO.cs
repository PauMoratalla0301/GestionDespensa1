using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearProveedorDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Nombre { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string CUIT { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Telefono { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Direccion { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Notas { get; set; }
    }
}
