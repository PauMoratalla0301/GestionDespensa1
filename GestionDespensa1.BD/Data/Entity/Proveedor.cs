using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    [Index(nameof(CUIT), Name = "Proveedor_UQ", IsUnique = true)]
    public class Proveedor : EntityBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "CUIT")]
        public string CUIT { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Notas")]
        public string Notas { get; set; }

        // Navigation properties
        public List<CompraProveedor> ComprasProveedor { get; set; } = new List<CompraProveedor>();


    }
}
