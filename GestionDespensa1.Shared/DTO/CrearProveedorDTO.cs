using System.ComponentModel.DataAnnotations;

public class CrearProveedorDTO
{
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
}
