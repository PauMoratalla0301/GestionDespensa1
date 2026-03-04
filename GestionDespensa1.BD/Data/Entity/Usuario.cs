// BD/Data/Entity/Usuario.cs
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.BD.Data.Entity
{
    [Index(nameof(Email), Name = "Usuario_Email_UQ", IsUnique = true)]
    public class Usuario : EntityBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Máximo número de caracteres {1}.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string PasswordHash { get; set; } = string.Empty; // Guardaremos HASH, no texto plano

        [Required]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Rol { get; set; } = "EMPLEADO"; // ADMIN / EMPLEADO

        [Required]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; } = "ACTIVO"; // ACTIVO / INACTIVO

        // Navigation properties
        public List<Venta> Ventas { get; set; } = new List<Venta>();
        public List<Caja> Cajas { get; set; } = new List<Caja>();
    }
}
