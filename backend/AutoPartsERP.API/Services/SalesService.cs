using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Sales;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Services;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;

    public SalesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<SalesInvoiceDto>> GetSalesAsync(string? search, string? status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 20)
    {
        var query = _context.SalesInvoices
            .Include(s => s.Customer)
            .Include(s => s.Warehouse)
            .Include(s => s.Items).ThenInclude(i => i.Part)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(x =>
                x.InvoiceNumber.ToLower().Contains(s) ||
                x.Customer.Name.ToLower().Contains(s) ||
                x.Customer.Phone.Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.PaymentStatus.ToLower() == status.ToLower() || x.Status.ToLower() == status.ToLower());
        }

        if (fromDate.HasValue)
            query = query.Where(x => x.SaleDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(x => x.SaleDate <= toDate.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.SaleDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SalesInvoiceDto
            {
                Id = s.Id,
                InvoiceNumber = s.InvoiceNumber,
                CustomerId = s.CustomerId,
                CustomerName = s.Customer.Name,
                CustomerPhone = s.Customer.Phone,
                WarehouseId = s.WarehouseId,
                WarehouseName = s.Warehouse.Name,
                SaleDate = s.SaleDate,
                SaleType = s.SaleType,
                SubTotal = s.SubTotal,
                DiscountAmount = s.DiscountAmount,
                TaxAmount = s.TaxAmount,
                TotalAmount = s.TotalAmount,
                PaidAmount = s.PaidAmount,
                DueAmount = s.DueAmount,
                PaymentMethod = s.PaymentMethod,
                PaymentStatus = s.PaymentStatus,
                Status = s.Status,
                Notes = s.Notes,
                Items = s.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartNumber = i.Part.PartNumber,
                    PartName = i.Part.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    DiscountPercent = i.DiscountPercent,
                    DiscountAmount = i.DiscountAmount,
                    TotalPrice = i.TotalPrice
                }).ToList()
            })
            .ToListAsync();

        return new PagedResponse<SalesInvoiceDto>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = items,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ApiResponse<SalesInvoiceDto>> GetSaleByIdAsync(int id)
    {
        var s = await _context.SalesInvoices
            .Include(x => x.Customer)
            .Include(x => x.Warehouse)
            .Include(x => x.Items).ThenInclude(i => i.Part)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (s == null) return ApiResponse<SalesInvoiceDto>.Fail("Sales invoice not found");

        return ApiResponse<SalesInvoiceDto>.Ok(new SalesInvoiceDto
        {
            Id = s.Id,
            InvoiceNumber = s.InvoiceNumber,
            CustomerId = s.CustomerId,
            CustomerName = s.Customer?.Name ?? "",
            CustomerPhone = s.Customer?.Phone ?? "",
            WarehouseId = s.WarehouseId,
            WarehouseName = s.Warehouse?.Name ?? "",
            SaleDate = s.SaleDate,
            SaleType = s.SaleType,
            SubTotal = s.SubTotal,
            DiscountAmount = s.DiscountAmount,
            TaxAmount = s.TaxAmount,
            TotalAmount = s.TotalAmount,
            PaidAmount = s.PaidAmount,
            DueAmount = s.DueAmount,
            PaymentMethod = s.PaymentMethod,
            PaymentStatus = s.PaymentStatus,
            Status = s.Status,
            Notes = s.Notes,
            Items = s.Items.Select(i => new SalesItemDto
            {
                Id = i.Id,
                PartId = i.PartId,
                PartNumber = i.Part?.PartNumber ?? "",
                PartName = i.Part?.Name ?? "",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercent = i.DiscountPercent,
                DiscountAmount = i.DiscountAmount,
                TotalPrice = i.TotalPrice
            }).ToList()
        });
    }

    public async Task<ApiResponse<SalesInvoiceDto>> CreateSaleAsync(CreateSalesInvoiceDto dto, int? userId)
    {
        if (!dto.Items.Any())
            return ApiResponse<SalesInvoiceDto>.Fail("Invoice must contain at least one item");

        var invoiceNo = "INV-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        decimal subTotal = 0;

        var salesItems = new List<SalesItem>();

        foreach (var it in dto.Items)
        {
            var part = await _context.Parts.FindAsync(it.PartId);
            if (part == null) return ApiResponse<SalesInvoiceDto>.Fail($"Part ID {it.PartId} not found");

            var itemTotal = (it.Quantity * it.UnitPrice) * (1 - (it.DiscountPercent / 100));
            subTotal += itemTotal;

            salesItems.Add(new SalesItem
            {
                PartId = it.PartId,
                Quantity = it.Quantity,
                UnitPrice = it.UnitPrice,
                DiscountPercent = it.DiscountPercent,
                DiscountAmount = (it.Quantity * it.UnitPrice) * (it.DiscountPercent / 100),
                TotalPrice = itemTotal
            });

            // Reduce Warehouse Stock
            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(ws => ws.WarehouseId == dto.WarehouseId && ws.PartId == it.PartId);

            if (stock != null)
            {
                stock.Quantity -= it.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }

            // Log Stock Movement
            await _context.StockMovements.AddAsync(new StockMovement
            {
                PartId = it.PartId,
                WarehouseId = dto.WarehouseId,
                MovementType = "Sale",
                Quantity = -it.Quantity,
                UnitCost = part.CostPrice,
                ReferenceNumber = invoiceNo,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        var totalAmount = subTotal - dto.DiscountAmount + dto.TaxAmount;
        var dueAmount = totalAmount - dto.PaidAmount;
        var paymentStatus = dueAmount <= 0 ? "Paid" : (dto.PaidAmount > 0 ? "Partial" : "Due");

        var invoice = new SalesInvoice
        {
            InvoiceNumber = invoiceNo,
            CustomerId = dto.CustomerId,
            WarehouseId = dto.WarehouseId,
            SaleDate = DateTime.UtcNow,
            SaleType = dto.SaleType,
            SubTotal = subTotal,
            DiscountAmount = dto.DiscountAmount,
            TaxAmount = dto.TaxAmount,
            TotalAmount = totalAmount,
            PaidAmount = dto.PaidAmount,
            DueAmount = dueAmount > 0 ? dueAmount : 0,
            PaymentMethod = dto.PaymentMethod,
            PaymentStatus = paymentStatus,
            Status = "Completed",
            Notes = dto.Notes,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            Items = salesItems
        };

        await _context.SalesInvoices.AddAsync(invoice);

        // Update customer balance
        var customer = await _context.Customers.FindAsync(dto.CustomerId);
        if (customer != null && dueAmount > 0)
        {
            customer.CurrentBalance += dueAmount;
        }

        await _context.SaveChangesAsync();

        return await GetSaleByIdAsync(invoice.Id);
    }
}
