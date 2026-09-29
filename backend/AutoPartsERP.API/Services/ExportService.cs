using ClosedXML.Excel;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Services;

public class ExportService : IExportService
{
    public byte[] ExportPartsToExcel(IEnumerable<PartDto> parts)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Parts Catalog");

        // Header Title
        worksheet.Cell(1, 1).Value = "Ainan International Auto Parts System (AIAPS) - Parts Catalog";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1B3A6B");
        worksheet.Range(1, 1, 1, 9).Merge();

        // Columns
        string[] headers = { "#", "Part Number", "OEM Number", "Name", "Category", "Brand", "Cost Price (BDT)", "Selling Price (BDT)", "Stock" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(3, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1B3A6B");
            cell.Style.Font.FontColor = XLColor.White;
        }

        int row = 4;
        int idx = 1;
        foreach (var p in parts)
        {
            worksheet.Cell(row, 1).Value = idx++;
            worksheet.Cell(row, 2).Value = p.PartNumber;
            worksheet.Cell(row, 3).Value = p.OEMNumber ?? "-";
            worksheet.Cell(row, 4).Value = p.Name;
            worksheet.Cell(row, 5).Value = p.CategoryName ?? "-";
            worksheet.Cell(row, 6).Value = p.BrandName ?? "-";
            worksheet.Cell(row, 7).Value = p.CostPrice;
            worksheet.Cell(row, 8).Value = p.SellingPrice;
            worksheet.Cell(row, 9).Value = p.TotalStock;

            if (row % 2 == 0)
            {
                worksheet.Range(row, 1, row, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFC");
            }
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportSalesToExcel(IEnumerable<SalesInvoiceDto> sales)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sales Report");

        worksheet.Cell(1, 1).Value = "Ainan International Auto Parts System (AIAPS) - Sales Invoices";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1B3A6B");
        worksheet.Range(1, 1, 1, 8).Merge();

        string[] headers = { "#", "Invoice No", "Date", "Customer", "Total Amount (BDT)", "Paid (BDT)", "Due (BDT)", "Status" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(3, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1B3A6B");
            cell.Style.Font.FontColor = XLColor.White;
        }

        int row = 4;
        int idx = 1;
        foreach (var s in sales)
        {
            worksheet.Cell(row, 1).Value = idx++;
            worksheet.Cell(row, 2).Value = s.InvoiceNumber;
            worksheet.Cell(row, 3).Value = s.SaleDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 4).Value = s.CustomerName;
            worksheet.Cell(row, 5).Value = s.TotalAmount;
            worksheet.Cell(row, 6).Value = s.PaidAmount;
            worksheet.Cell(row, 7).Value = s.DueAmount;
            worksheet.Cell(row, 8).Value = s.PaymentStatus;

            if (row % 2 == 0)
            {
                worksheet.Range(row, 1, row, 8).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFC");
            }
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
