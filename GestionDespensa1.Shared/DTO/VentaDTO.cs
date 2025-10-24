using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de cliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria.")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio.")]
        public decimal MontoPagado { get; set; }

        [Required(ErrorMessage = "El saldo pendiente es obligatorio.")]
        public decimal SaldoPendiente { get; set; }

        // NUEVOS CAMPOS
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [MaxLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string MetodoPago { get; set; } = "Efectivo";

        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string? Notas { get; set; }

        // Productos de la venta
        public List<DetalleVentaDTO> DetallesVenta { get; set; } = new List<DetalleVentaDTO>();

        // Información del cliente (opcional, para mostrar)
        public string? NombreCliente { get; set; }
    }
}