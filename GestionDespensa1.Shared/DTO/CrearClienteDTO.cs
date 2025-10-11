using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearClienteDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string Dni { get; set; } = string.Empty;

        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "El saldo pendiente es obligatorio.")]
        public decimal SaldoPendiente { get; set; }
    }
}