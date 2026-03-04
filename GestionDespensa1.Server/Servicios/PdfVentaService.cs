using iTextSharp.text;
using iTextSharp.text.pdf;
using GestionDespensa1.Shared.DTO;
using System.IO;

namespace GestionDespensa1.Server.Servicios
{
    public class PdfVentaService
    {
        public byte[] GenerarComprobanteVenta(VentaDTO venta, List<DetalleVentaDTO> detalles, string nombreCliente)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Crear documento más pequeño (tamaño ticket)
                Document document = new Document(new Rectangle(226f, 500f), 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Colores
                BaseColor colorVerde = new BaseColor(39, 174, 96);      // #27ae60
                BaseColor colorNegro = new BaseColor(0, 0, 0);
                BaseColor colorGris = new BaseColor(128, 128, 128);

                // Fuentes
                iTextSharp.text.Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, colorVerde);
                iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, colorNegro);
                iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, colorNegro);
                iTextSharp.text.Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 6, colorGris);

                // Título del negocio
                Paragraph negocio = new Paragraph("GESTIÓN DESPENSA", tituloFont);
                negocio.Alignment = Element.ALIGN_CENTER;
                negocio.SpacingAfter = 5f;
                document.Add(negocio);

                // Línea separadora
                Paragraph linea = new Paragraph(new string('-', 32), smallFont);
                linea.Alignment = Element.ALIGN_CENTER;
                linea.SpacingAfter = 5f;
                document.Add(linea);

                // Título comprobante
                Paragraph titulo = new Paragraph("COMPROBANTE DE VENTA", boldFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 10f;
                document.Add(titulo);

                // Información de la venta
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100f;
                infoTable.SetWidths(new float[] { 40f, 60f });

                infoTable.AddCell(GetCell("N° Venta:", boldFont));
                infoTable.AddCell(GetCell($"#{venta.Id}", normalFont));

                infoTable.AddCell(GetCell("Fecha:", boldFont));
                infoTable.AddCell(GetCell(venta.FechaVenta.ToString("dd/MM/yyyy HH:mm"), normalFont));

                infoTable.AddCell(GetCell("Cliente:", boldFont));
                infoTable.AddCell(GetCell(nombreCliente, normalFont));

                infoTable.SpacingAfter = 10f;
                document.Add(infoTable);

                // Línea separadora
                linea = new Paragraph(new string('-', 32), smallFont);
                linea.Alignment = Element.ALIGN_CENTER;
                linea.SpacingAfter = 5f;
                document.Add(linea);

                // Tabla de productos
                PdfPTable productTable = new PdfPTable(3);
                productTable.WidthPercentage = 100f;
                productTable.SetWidths(new float[] { 50f, 20f, 30f });

                // Encabezados
                productTable.AddCell(GetCell("Producto", boldFont));
                productTable.AddCell(GetCell("Cant.", boldFont, Element.ALIGN_CENTER));
                productTable.AddCell(GetCell("Precio", boldFont, Element.ALIGN_RIGHT));

                decimal total = 0;
                foreach (var detalle in detalles)
                {
                    productTable.AddCell(GetCell(detalle.DescripcionProducto ?? "Producto", normalFont));

                    PdfPCell cellCantidad = GetCell(detalle.Cantidad.ToString(), normalFont);
                    cellCantidad.HorizontalAlignment = Element.ALIGN_CENTER;
                    productTable.AddCell(cellCantidad);

                    decimal subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    PdfPCell cellPrecio = GetCell(subtotal.ToString("C"), normalFont);
                    cellPrecio.HorizontalAlignment = Element.ALIGN_RIGHT;
                    productTable.AddCell(cellPrecio);

                    total += subtotal;
                }

                productTable.SpacingAfter = 5f;
                document.Add(productTable);

                // Línea separadora
                linea = new Paragraph(new string('-', 32), smallFont);
                linea.Alignment = Element.ALIGN_CENTER;
                linea.SpacingAfter = 5f;
                document.Add(linea);

                // Total
                PdfPTable totalTable = new PdfPTable(2);
                totalTable.WidthPercentage = 100f;
                totalTable.SetWidths(new float[] { 60f, 40f });

                totalTable.AddCell(GetCell("TOTAL:", boldFont, Element.ALIGN_RIGHT));

                PdfPCell cellTotal = GetCell(total.ToString("C"), boldFont);
                cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalTable.AddCell(cellTotal);

                totalTable.SpacingAfter = 5f;
                document.Add(totalTable);

                // Método de pago
                Paragraph pago = new Paragraph($"Método de pago: {venta.MetodoPago}", normalFont);
                pago.Alignment = Element.ALIGN_CENTER;
                pago.SpacingAfter = 10f;
                document.Add(pago);

                // Estado
                Paragraph estado = new Paragraph($"Estado: {venta.Estado}", normalFont);
                estado.Alignment = Element.ALIGN_CENTER;
                estado.SpacingAfter = 15f;
                document.Add(estado);

                // Línea separadora
                linea = new Paragraph(new string('-', 32), smallFont);
                linea.Alignment = Element.ALIGN_CENTER;
                linea.SpacingAfter = 5f;
                document.Add(linea);

                // Pie de página
                Paragraph gracias = new Paragraph("¡Gracias por su compra!", boldFont);
                gracias.Alignment = Element.ALIGN_CENTER;
                gracias.SpacingAfter = 3f;
                document.Add(gracias);

                Paragraph fecha = new Paragraph(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), smallFont);
                fecha.Alignment = Element.ALIGN_CENTER;
                document.Add(fecha);

                document.Close();
                return ms.ToArray();
            }
        }

        private PdfPCell GetCell(string texto, iTextSharp.text.Font font, int alignment = Element.ALIGN_LEFT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto, font));
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 2f;
            cell.HorizontalAlignment = alignment;
            return cell;
        }
    }
}