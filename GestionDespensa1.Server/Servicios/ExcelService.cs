using OfficeOpenXml;
using OfficeOpenXml.Style;
using GestionDespensa1.Shared.DTO;
using System.Drawing;

namespace GestionDespensa1.Server.Servicios
{
    public class ExcelService
    {
        public byte[] GenerarExcelVentas(ReporteVentasDTO reporte)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Ventas");

                // Título
                worksheet.Cells["A1"].Value = "REPORTE DE VENTAS";
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Período
                worksheet.Cells["A2"].Value = $"Período: {reporte.FechaInicio:dd/MM/yyyy} - {reporte.FechaFin:dd/MM/yyyy}";
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells["A2"].Style.Font.Bold = true;

                // Resumen
                worksheet.Cells["A4"].Value = "RESUMEN";
                worksheet.Cells["A4"].Style.Font.Bold = true;
                worksheet.Cells["A4"].Style.Font.Size = 12;

                worksheet.Cells["A5"].Value = "Total Ventas:";
                worksheet.Cells["B5"].Value = reporte.TotalIngresos;
                worksheet.Cells["B5"].Style.Numberformat.Format = "$ #,##0.00";

                worksheet.Cells["A6"].Value = "Cantidad de Ventas:";
                worksheet.Cells["B6"].Value = reporte.TotalVentas;

                worksheet.Cells["A7"].Value = "Total Efectivo:";
                worksheet.Cells["B7"].Value = reporte.TotalEfectivo;
                worksheet.Cells["B7"].Style.Numberformat.Format = "$ #,##0.00";

                worksheet.Cells["A8"].Value = "Total Tarjeta:";
                worksheet.Cells["B8"].Value = reporte.TotalTarjeta;
                worksheet.Cells["B8"].Style.Numberformat.Format = "$ #,##0.00";

                worksheet.Cells["A9"].Value = "Total Transferencia:";
                worksheet.Cells["B9"].Value = reporte.TotalTransferencia;
                worksheet.Cells["B9"].Style.Numberformat.Format = "$ #,##0.00";

                // Encabezados de tabla
                worksheet.Cells["A11"].Value = "Fecha";
                worksheet.Cells["B11"].Value = "Cliente";
                worksheet.Cells["C11"].Value = "Total";
                worksheet.Cells["D11"].Value = "Método Pago";
                worksheet.Cells["E11"].Value = "Estado";

                using (var range = worksheet.Cells["A11:E11"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Datos
                int row = 12;
                foreach (var venta in reporte.Ventas)
                {
                    worksheet.Cells[$"A{row}"].Value = venta.Fecha.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cells[$"B{row}"].Value = venta.Cliente;
                    worksheet.Cells[$"C{row}"].Value = venta.Total;
                    worksheet.Cells[$"C{row}"].Style.Numberformat.Format = "$ #,##0.00";
                    worksheet.Cells[$"D{row}"].Value = venta.MetodoPago;
                    worksheet.Cells[$"E{row}"].Value = venta.Estado;
                    row++;
                }

                // Autoajustar columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public byte[] GenerarExcelProductos(ReporteProductosDTO reporte)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Productos Más Vendidos");

                // Título
                worksheet.Cells["A1"].Value = "PRODUCTOS MÁS VENDIDOS";
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Período
                worksheet.Cells["A2"].Value = $"Período: {reporte.FechaInicio:dd/MM/yyyy} - {reporte.FechaFin:dd/MM/yyyy}";
                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2"].Style.Font.Bold = true;

                // Encabezados
                worksheet.Cells["A4"].Value = "Producto";
                worksheet.Cells["B4"].Value = "Categoría";
                worksheet.Cells["C4"].Value = "Cantidad Vendida";
                worksheet.Cells["D4"].Value = "Total Vendido";

                using (var range = worksheet.Cells["A4:D4"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Datos
                int row = 5;
                foreach (var prod in reporte.Productos)
                {
                    worksheet.Cells[$"A{row}"].Value = prod.Producto;
                    worksheet.Cells[$"B{row}"].Value = prod.Categoria;
                    worksheet.Cells[$"C{row}"].Value = prod.CantidadVendida;
                    worksheet.Cells[$"D{row}"].Value = prod.TotalVendido;
                    worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "$ #,##0.00";
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public byte[] GenerarExcelDeudores(ReporteDeudoresDTO reporte)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Clientes Deudores");

                // Título
                worksheet.Cells["A1"].Value = "CLIENTES DEUDORES";
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Resumen
                worksheet.Cells["A3"].Value = "Total General:";
                worksheet.Cells["B3"].Value = reporte.TotalGeneral;
                worksheet.Cells["B3"].Style.Numberformat.Format = "$ #,##0.00";
                worksheet.Cells["A4"].Value = "Cantidad de Deudores:";
                worksheet.Cells["B4"].Value = reporte.CantidadDeudores;

                // Encabezados
                worksheet.Cells["A6"].Value = "Cliente";
                worksheet.Cells["B6"].Value = "Teléfono";
                worksheet.Cells["C6"].Value = "Email";
                worksheet.Cells["D6"].Value = "Saldo Pendiente";
                worksheet.Cells["E6"].Value = "Cant. Ventas";

                using (var range = worksheet.Cells["A6:E6"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Datos
                int row = 7;
                foreach (var deudor in reporte.Deudores)
                {
                    worksheet.Cells[$"A{row}"].Value = deudor.Cliente;
                    worksheet.Cells[$"B{row}"].Value = deudor.Telefono;
                    worksheet.Cells[$"C{row}"].Value = deudor.Email;
                    worksheet.Cells[$"D{row}"].Value = deudor.SaldoPendiente;
                    worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "$ #,##0.00";
                    worksheet.Cells[$"E{row}"].Value = deudor.Ventas.Count;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray();
            }
        }

        public byte[] GenerarExcelStockBajo(ReporteStockBajoDTO reporte)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Stock Bajo");

                // Título
                worksheet.Cells["A1"].Value = "PRODUCTOS CON STOCK BAJO";
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Resumen
                worksheet.Cells["A3"].Value = "Total Productos con Stock Bajo:";
                worksheet.Cells["B3"].Value = reporte.Productos.Count;

                // Encabezados
                worksheet.Cells["A5"].Value = "Producto";
                worksheet.Cells["B5"].Value = "Categoría";
                worksheet.Cells["C5"].Value = "Stock Actual";
                worksheet.Cells["D5"].Value = "Stock Mínimo";
                worksheet.Cells["E5"].Value = "Faltante";

                using (var range = worksheet.Cells["A5:E5"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Datos
                int row = 6;
                foreach (var prod in reporte.Productos)
                {
                    worksheet.Cells[$"A{row}"].Value = prod.Producto;
                    worksheet.Cells[$"B{row}"].Value = prod.Categoria;
                    worksheet.Cells[$"C{row}"].Value = prod.StockActual;
                    worksheet.Cells[$"D{row}"].Value = prod.StockMinimo;
                    worksheet.Cells[$"E{row}"].Value = prod.Faltante;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                return package.GetAsByteArray();
            }
        }
    }
}