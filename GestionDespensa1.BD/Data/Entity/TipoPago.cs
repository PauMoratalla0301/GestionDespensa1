using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class TipoPago : EntityBase
    {
        [Required(ErrorMessage = "La descripción del tipo de pago es obligatoria.")]
        [MaxLength(50, ErrorMessage = "La descripción no puede superar los 50 caracteres.")]
        [Display(Name = "Tipo de Pago")]
        public string Descripcion { get; set; }//Efectivo, Tarjeta, Transferencia, etc.

        // Relaciones 1:N con Pago
        public List<Pago> Pagos { get; set; } = new List<Pago>();

        public override string ToString() => Descripcion;
    }
}
