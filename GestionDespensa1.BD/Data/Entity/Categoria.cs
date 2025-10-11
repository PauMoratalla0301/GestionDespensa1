using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    [Index(nameof(NombreCategoria), Name = "Categoria_UQ", IsUnique = true)]
    public class Categoria : EntityBase
    {
        [Required(ErrorMessage = "El nombre de categoría es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Nombre Categoría")]
        public string NombreCategoria { get; set; }

        // Navigation properties
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
