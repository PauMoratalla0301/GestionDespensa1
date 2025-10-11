using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de categoría es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string NombreCategoria { get; set; }
    }
}
