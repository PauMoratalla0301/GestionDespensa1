using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class ProveedorDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string CUIT { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; } = "Activo";

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Notas { get; set; } = string.Empty;

        // Nueva propiedad para productos del proveedor
        public List<CompraProveedorDTO> Compras { get; set; } = new List<CompraProveedorDTO>();
        public string Productos { get; set; } = string.Empty;

        // Propiedad para contacto combinado
        public string ContactoInfo
        {
            get
            {
                var contactos = new List<string>();
                if (!string.IsNullOrEmpty(Telefono))
                    contactos.Add(Telefono);
                if (!string.IsNullOrEmpty(Email))
                    contactos.Add(Email);

                return string.Join(" ", contactos);
            }
        }
    }
}