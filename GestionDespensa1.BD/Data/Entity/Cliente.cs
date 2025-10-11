using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    [Index(nameof(Dni), Name = "Cliente_UQ", IsUnique = true)]
    public class Cliente : EntityBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "DNI")]
        public string Dni { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        // EMAIL COMO OPCIONAL - VERIFICA QUE TENGA EL ?
        [MaxLength(100, ErrorMessage = "Máximo número de caracteres {1}.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [Display(Name = "Email")]
        public string? Email { get; set; } // ← Asegúrate del ? aquí

        [Required(ErrorMessage = "El saldo pendiente es obligatorio.")]
        [Display(Name = "Saldo Pendiente")]
        [Precision(18, 2)]
        public decimal SaldoPendiente { get; set; }

        // Navigation properties
        public List<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
