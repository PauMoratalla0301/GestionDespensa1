using System.ComponentModel.DataAnnotations;

public class CrearMovimientoStockDTO
{
    [Required(ErrorMessage = "El producto es obligatorio.")]
    public int IdProducto { get; set; }

    [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
    [MaxLength(20)]
    public string Tipo { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    [MaxLength(500)]
    public string? Referencia { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    public int StockAnterior { get; set; }
    public int StockNuevo { get; set; }
}
