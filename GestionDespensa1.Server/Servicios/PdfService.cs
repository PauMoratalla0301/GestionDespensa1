using iTextSharp.text;
using iTextSharp.text.pdf;
using GestionDespensa1.Shared.DTO;
using System.IO;

namespace GestionDespensa1.Server.Servicios
{
    public class PdfService
    {
        public byte[] GenerarComprobanteCompra(CompraProveedorDTO compra, List<DetalleCompraProveedorDTO> detalles)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Crear documento
                Document document = new Document(PageSize.A4, 25f, 25f, 30f, 30f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Colores en RGB
                BaseColor colorVerdeOscuro = new BaseColor(0, 100, 0);      // Verde oscuro
                BaseColor colorNegro = new BaseColor(0, 0, 0);              // Negro
                BaseColor colorBlanco = new BaseColor(255, 255, 255);       // Blanco
                BaseColor colorGris = new BaseColor(128, 128, 128);         // Gris
                BaseColor colorGrisClaro = new BaseColor(211, 211, 211);    // Gris claro

                // Fuentes
                iTextSharp.text.Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, colorVerdeOscuro);
                iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, colorNegro);
                iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, colorNegro);
                iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, colorBlanco);

                // Título
                Paragraph titulo = new Paragraph("COMPROBANTE DE COMPRA", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20f;
                document.Add(titulo);

                // Información de la compra
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100f;
                infoTable.SetWidths(new float[] { 30f, 70f });

                infoTable.AddCell(GetCell("N° Compra:", boldFont));
                infoTable.AddCell(GetCell($"#{compra.Id}", normalFont));

                infoTable.AddCell(GetCell("Proveedor:", boldFont));
                infoTable.AddCell(GetCell(compra.NombreProveedor ?? "", normalFont));

                infoTable.AddCell(GetCell("Fecha:", boldFont));
                infoTable.AddCell(GetCell(compra.FechaCompra.ToString("dd/MM/yyyy HH:mm"), normalFont));

                infoTable.AddCell(GetCell("Estado:", boldFont));
                infoTable.AddCell(GetCell(compra.Estado, normalFont));

                infoTable.AddCell(GetCell("Método Pago:", boldFont));
                infoTable.AddCell(GetCell(compra.MetodoPago ?? "EFECTIVO", normalFont));

                infoTable.SpacingAfter = 20f;
                document.Add(infoTable);

                // Tabla de productos
                PdfPTable productTable = new PdfPTable(4);
                productTable.WidthPercentage = 100f;
                productTable.SetWidths(new float[] { 40f, 15f, 20f, 25f });

                // Encabezados con fondo gris
                PdfPCell cellHeader1 = GetCell("Producto", headerFont);
                cellHeader1.BackgroundColor = colorGris;
                productTable.AddCell(cellHeader1);

                PdfPCell cellHeader2 = GetCell("Cantidad", headerFont);
                cellHeader2.BackgroundColor = colorGris;
                cellHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                productTable.AddCell(cellHeader2);

                PdfPCell cellHeader3 = GetCell("Precio Unit.", headerFont);
                cellHeader3.BackgroundColor = colorGris;
                cellHeader3.HorizontalAlignment = Element.ALIGN_RIGHT;
                productTable.AddCell(cellHeader3);

                PdfPCell cellHeader4 = GetCell("Subtotal", headerFont);
                cellHeader4.BackgroundColor = colorGris;
                cellHeader4.HorizontalAlignment = Element.ALIGN_RIGHT;
                productTable.AddCell(cellHeader4);

                // Detalles
                decimal total = 0;
                foreach (var detalle in detalles)
                {
                    productTable.AddCell(GetCell(detalle.DescripcionProducto ?? "Producto", normalFont));

                    PdfPCell cellCantidad = GetCell(detalle.Cantidad.ToString(), normalFont);
                    cellCantidad.HorizontalAlignment = Element.ALIGN_CENTER;
                    productTable.AddCell(cellCantidad);

                    PdfPCell cellPrecio = GetCell(detalle.PrecioUnitario.ToString("C"), normalFont);
                    cellPrecio.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productTable.AddCell(cellPrecio);

                    decimal subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    PdfPCell cellSubtotal = GetCell(subtotal.ToString("C"), normalFont);
                    cellSubtotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productTable.AddCell(cellSubtotal);

                    total += subtotal;
                }

                // Fila de total
                PdfPCell cellTotalLabel = GetCell("TOTAL", boldFont);
                cellTotalLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellTotalLabel.Colspan = 3;
                productTable.AddCell(cellTotalLabel);

                PdfPCell cellTotalValue = GetCell(total.ToString("C"), boldFont);
                cellTotalValue.BackgroundColor = colorGrisClaro;
                cellTotalValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                productTable.AddCell(cellTotalValue);

                productTable.SpacingAfter = 20f;
                document.Add(productTable);

                // Pie de página
                Paragraph pie = new Paragraph("Gracias por su compra", normalFont);
                pie.Alignment = Element.ALIGN_CENTER;
                pie.SpacingBefore = 20f;
                document.Add(pie);

                Paragraph fecha = new Paragraph($"Documento generado el {DateTime.Now:dd/MM/yyyy HH:mm}", normalFont);
                fecha.Alignment = Element.ALIGN_CENTER;
                fecha.SpacingBefore = 5f;
                document.Add(fecha);

                document.Close();
                return ms.ToArray();
            }
        }

        private PdfPCell GetCell(string texto, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto, font));
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 5f;
            return cell;
        }
    }
}