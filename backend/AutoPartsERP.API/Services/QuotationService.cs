using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Sales;

namespace AutoPartsERP.API.Services;

public class QuotationService : IQuotationService
{
    private readonly AppDbContext _context;

    public QuotationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<QuotationResultDto>> CreateAsync(CreateQuotationDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            return ApiResponse<QuotationResultDto>.Fail("Quotation must contain at least one item");

        if (dto.Items.Any(i => i.Quantity <= 0))
            return ApiResponse<QuotationResultDto>.Fail("Quantity must be greater than zero");

        if (dto.Items.Any(i => i.UnitPrice.HasValue && i.UnitPrice.Value < 0))
            return ApiResponse<QuotationResultDto>.Fail("Unit price cannot be negative");

        if (dto.Items.GroupBy(i => i.PartId).Any(g => g.Count() > 1))
            return ApiResponse<QuotationResultDto>.Fail("Each part may appear only once in a quotation");

        if (dto.DiscountAmount < 0 || dto.TaxAmount < 0)
            return ApiResponse<QuotationResultDto>.Fail("Discount and tax cannot be negative");

        if (dto.ValidDays <= 0)
            return ApiResponse<QuotationResultDto>.Fail("Validity must be at least one day");

        if (!await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId))
            return ApiResponse<QuotationResultDto>.Fail($"Customer ID {dto.CustomerId} not found");

        var partIds = dto.Items.Select(i => i.PartId).ToList();
        var parts = await _context.Parts.Where(p => partIds.Contains(p.Id)).ToListAsync();

        var items = new List<QuotationItem>();
        decimal subTotal = 0;

        foreach (var it in dto.Items)
        {
            var part = parts.FirstOrDefault(p => p.Id == it.PartId);
            if (part == null)
                return ApiResponse<QuotationResultDto>.Fail($"Part ID {it.PartId} not found");

            var unitPrice = Math.Round(it.UnitPrice ?? part.SellingPrice, 2);
            var lineTotal = Math.Round(it.Quantity * unitPrice, 2);
            subTotal += lineTotal;

            items.Add(new QuotationItem
            {
                PartId = it.PartId,
                Quantity = it.Quantity,
                UnitPrice = unitPrice,
                TotalPrice = lineTotal
            });
        }

        var discount = Math.Round(dto.DiscountAmount, 2);
        var tax = Math.Round(dto.TaxAmount, 2);

        if (discount > subTotal)
            return ApiResponse<QuotationResultDto>.Fail("Discount cannot exceed the subtotal");

        var quotation = new Quotation
        {
            QuotationNumber = "QT-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
            CustomerId = dto.CustomerId,
            ExpiryDate = DateTime.UtcNow.AddDays(dto.ValidDays),
            SubTotal = subTotal,
            DiscountAmount = discount,
            TaxAmount = tax,
            TotalAmount = subTotal - discount + tax,
            Status = "Pending",
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            Items = items
        };

        await _context.Quotations.AddAsync(quotation);
        await _context.SaveChangesAsync();

        return ApiResponse<QuotationResultDto>.Ok(new QuotationResultDto
        {
            Id = quotation.Id,
            QuotationNumber = quotation.QuotationNumber,
            CustomerId = quotation.CustomerId,
            SubTotal = quotation.SubTotal,
            DiscountAmount = quotation.DiscountAmount,
            TaxAmount = quotation.TaxAmount,
            TotalAmount = quotation.TotalAmount,
            ExpiryDate = quotation.ExpiryDate
        });
    }
}