using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class Venta : EntityBase
    {
        [Required(ErrorMessage = "El ID de cliente es obligatorio.")]
        [Display(Name = "ID Cliente")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria.")]
        [Display(Name = "Fecha Venta")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        [Display(Name = "Total")]
        [Precision(18, 2)]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio.")]
        [Display(Name = "Monto Pagado")]
        [Precision(18, 2)]
        public decimal MontoPagado { get; set; }

        [Required(ErrorMessage = "El saldo pendiente es obligatorio.")]
        [Display(Name = "Saldo Pendiente")]
        [Precision(18, 2)]
        public decimal SaldoPendiente { get; set; }

        // Navigation properties
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }
        public List<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();


    }
}
