using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Purchase;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Services;

public class PurchaseService : IPurchaseService
{
    private readonly AppDbContext _context;

    public PurchaseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<PurchaseInvoiceDto>> GetPurchasesAsync(string? search, string? status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 20)
    {
        var query = _context.PurchaseInvoices
            .Include(p => p.Supplier)
            .Include(p => p.Warehouse)
            .Include(p => p.Items).ThenInclude(i => i.Part)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(x =>
                x.InvoiceNumber.ToLower().Contains(s) ||
                x.Supplier.Name.ToLower().Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.PaymentStatus.ToLower() == status.ToLower() || x.Status.ToLower() == status.ToLower());
        }

        if (fromDate.HasValue) query = query.Where(x => x.PurchaseDate >= fromDate.Value);
        if (toDate.HasValue) query = query.Where(x => x.PurchaseDate <= toDate.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.PurchaseDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PurchaseInvoiceDto
            {
                Id = p.Id,
                InvoiceNumber = p.InvoiceNumber,
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier.Name,
                WarehouseId = p.WarehouseId,
                WarehouseName = p.Warehouse.Name,
                PurchaseDate = p.PurchaseDate,
                SubTotal = p.SubTotal,
                DiscountAmount = p.DiscountAmount,
                TaxAmount = p.TaxAmount,
                ShippingCost = p.ShippingCost,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                DueAmount = p.DueAmount,
                PaymentStatus = p.PaymentStatus,
                Status = p.Status,
                Items = p.Items.Select(i => new PurchaseItemDto
                {
                    Id = i.Id,
                    PartId = i.PartId,
                    PartNumber = i.Part.PartNumber,
                    PartName = i.Part.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalCost = i.TotalCost
                }).ToList()
            })
            .ToListAsync();

        return new PagedResponse<PurchaseInvoiceDto>
        {
            Success = true,
            Message = "Purchases retrieved successfully",
            Data = items,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ApiResponse<PurchaseInvoiceDto>> GetPurchaseByIdAsync(int id)
    {
        var p = await _context.PurchaseInvoices
            .Include(x => x.Supplier)
            .Include(x => x.Warehouse)
            .Include(x => x.Items).ThenInclude(i => i.Part)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p == null) return ApiResponse<PurchaseInvoiceDto>.Fail("Purchase invoice not found");

        return ApiResponse<PurchaseInvoiceDto>.Ok(new PurchaseInvoiceDto
        {
            Id = p.Id,
            InvoiceNumber = p.InvoiceNumber,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier?.Name ?? "",
            WarehouseId = p.WarehouseId,
            WarehouseName = p.Warehouse?.Name ?? "",
            PurchaseDate = p.PurchaseDate,
            SubTotal = p.SubTotal,
            DiscountAmount = p.DiscountAmount,
            TaxAmount = p.TaxAmount,
            ShippingCost = p.ShippingCost,
            TotalAmount = p.TotalAmount,
            PaidAmount = p.PaidAmount,
            DueAmount = p.DueAmount,
            PaymentStatus = p.PaymentStatus,
            Status = p.Status,
            Items = p.Items.Select(i => new PurchaseItemDto
            {
                Id = i.Id,
                PartId = i.PartId,
                PartNumber = i.Part?.PartNumber ?? "",
                PartName = i.Part?.Name ?? "",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalCost = i.TotalCost
            }).ToList()
        });
    }

    public async Task<ApiResponse<PurchaseInvoiceDto>> CreatePurchaseAsync(CreatePurchaseInvoiceDto dto, int? userId)
    {
        if (!dto.Items.Any()) return ApiResponse<PurchaseInvoiceDto>.Fail("Purchase must contain at least one item");

        var invoiceNo = "PO-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
        decimal subTotal = 0;
        var purchaseItems = new List<PurchaseItem>();

        foreach (var it in dto.Items)
        {
            var part = await _context.Parts.FindAsync(it.PartId);
            if (part == null) return ApiResponse<PurchaseInvoiceDto>.Fail($"Part ID {it.PartId} not found");

            var itemCost = it.Quantity * it.UnitPrice;
            subTotal += itemCost;

            purchaseItems.Add(new PurchaseItem
            {
                PartId = it.PartId,
                Quantity = it.Quantity,
                UnitPrice = it.UnitPrice,
                TotalCost = itemCost
            });

            // Update Stock
            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(ws => ws.WarehouseId == dto.WarehouseId && ws.PartId == it.PartId);

            if (stock != null)
            {
                stock.Quantity += it.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                await _context.WarehouseStocks.AddAsync(new WarehouseStock
                {
                    WarehouseId = dto.WarehouseId,
                    PartId = it.PartId,
                    Quantity = it.Quantity,
                    LastUpdated = DateTime.UtcNow
                });
            }

            // Update Part Cost Price if needed
            part.CostPrice = it.UnitPrice;

            // Log Movement
            await _context.StockMovements.AddAsync(new StockMovement
            {
                PartId = it.PartId,
                WarehouseId = dto.WarehouseId,
                MovementType = "Purchase",
                Quantity = it.Quantity,
                UnitCost = it.UnitPrice,
                ReferenceNumber = invoiceNo,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        var totalAmount = subTotal - dto.DiscountAmount + dto.TaxAmount + dto.ShippingCost;
        var dueAmount = totalAmount - dto.PaidAmount;
        var paymentStatus = dueAmount <= 0 ? "Paid" : (dto.PaidAmount > 0 ? "Partial" : "Due");

        var invoice = new PurchaseInvoice
        {
            InvoiceNumber = invoiceNo,
            SupplierId = dto.SupplierId,
            WarehouseId = dto.WarehouseId,
            PurchaseDate = DateTime.UtcNow,
            SubTotal = subTotal,
            DiscountAmount = dto.DiscountAmount,
            TaxAmount = dto.TaxAmount,
            ShippingCost = dto.ShippingCost,
            TotalAmount = totalAmount,
            PaidAmount = dto.PaidAmount,
            DueAmount = dueAmount > 0 ? dueAmount : 0,
            PaymentStatus = paymentStatus,
            Status = "Received",
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            Items = purchaseItems
        };

        await _context.PurchaseInvoices.AddAsync(invoice);

        var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
        if (supplier != null && dueAmount > 0)
        {
            supplier.CurrentBalance += dueAmount;
        }

        await _context.SaveChangesAsync();

        return await GetPurchaseByIdAsync(invoice.Id);
    }
}
