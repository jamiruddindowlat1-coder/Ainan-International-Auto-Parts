using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Purchase;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Services;

public class PurchaseReturnService : IPurchaseReturnService
{
    private readonly AppDbContext _context;

    public PurchaseReturnService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PurchaseReturnResultDto>> CreateAsync(CreatePurchaseReturnDto dto, int? userId)
    {
        if (dto.Items == null || !dto.Items.Any())
            return ApiResponse<PurchaseReturnResultDto>.Fail("Return must contain at least one item");

        if (dto.Items.Any(i => i.Quantity <= 0))
            return ApiResponse<PurchaseReturnResultDto>.Fail("Quantity must be greater than zero");

        if (dto.Items.GroupBy(i => i.PartId).Any(g => g.Count() > 1))
            return ApiResponse<PurchaseReturnResultDto>.Fail("Each part may appear only once in a return");

        if (!await _context.Warehouses.AnyAsync(w => w.Id == dto.WarehouseId))
            return ApiResponse<PurchaseReturnResultDto>.Fail($"Warehouse ID {dto.WarehouseId} not found");

        var invoice = await _context.PurchaseInvoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == dto.PurchaseInvoiceId);

        if (invoice == null)
            return ApiResponse<PurchaseReturnResultDto>.Fail($"Purchase invoice ID {dto.PurchaseInvoiceId} not found");

        if (invoice.Status == "Cancelled")
            return ApiResponse<PurchaseReturnResultDto>.Fail("Cannot return items of a cancelled invoice");

        var priorItems = await _context.PurchaseReturns
            .Where(r => r.PurchaseInvoiceId == invoice.Id && r.Status != "Cancelled")
            .SelectMany(r => r.Items)
            .Select(i => new { i.PartId, i.Quantity })
            .ToListAsync();

        var factor = invoice.SubTotal > 0 ? invoice.TotalAmount / invoice.SubTotal : 1m;
        var returnNo = "PR-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

        var returnItems = new List<PurchaseReturnItem>();
        decimal total = 0;

        foreach (var it in dto.Items)
        {
            var boughtRows = invoice.Items.Where(x => x.PartId == it.PartId).ToList();
            if (!boughtRows.Any())
                return ApiResponse<PurchaseReturnResultDto>.Fail($"Part ID {it.PartId} was not bought on this invoice");

            var boughtQty = boughtRows.Sum(x => x.Quantity);
            var alreadyReturned = priorItems.Where(p => p.PartId == it.PartId).Sum(p => p.Quantity);
            if (alreadyReturned + it.Quantity > boughtQty)
                return ApiResponse<PurchaseReturnResultDto>.Fail(
                    $"Part ID {it.PartId}: cannot return {it.Quantity}; bought {boughtQty}, already returned {alreadyReturned}");

            var stock = await _context.WarehouseStocks
                .FirstOrDefaultAsync(ws => ws.WarehouseId == dto.WarehouseId && ws.PartId == it.PartId);

            if (stock == null || stock.Quantity < it.Quantity)
                return ApiResponse<PurchaseReturnResultDto>.Fail($"Insufficient stock for part ID {it.PartId} in this warehouse");

            var unitNet = boughtRows.Sum(x => x.TotalCost) / boughtQty;
            var unitRefund = Math.Round(unitNet * factor, 2);
            var lineTotal = Math.Round(it.Quantity * unitNet * factor, 2);
            total += lineTotal;

            returnItems.Add(new PurchaseReturnItem
            {
                PartId = it.PartId,
                Quantity = it.Quantity,
                UnitCost = unitRefund,
                TotalCost = lineTotal
            });

            stock.Quantity -= it.Quantity;
            stock.LastUpdated = DateTime.UtcNow;

            await _context.StockMovements.AddAsync(new StockMovement
            {
                PartId = it.PartId,
                WarehouseId = dto.WarehouseId,
                MovementType = "PurchaseReturn",
                Quantity = -it.Quantity,
                UnitCost = unitRefund,
                ReferenceNumber = returnNo,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        var supplier = await _context.Suppliers.FindAsync(invoice.SupplierId);
        decimal applied = 0;
        if (supplier != null && supplier.CurrentBalance > 0)
        {
            applied = Math.Min(total, supplier.CurrentBalance);
            supplier.CurrentBalance -= applied;
        }
        var cashRefund = total - applied;

        var purchaseReturn = new PurchaseReturn
        {
            ReturnNumber = returnNo,
            PurchaseInvoiceId = invoice.Id,
            SupplierId = invoice.SupplierId,
            WarehouseId = dto.WarehouseId,
            ReturnDate = DateTime.UtcNow,
            TotalRefundAmount = total,
            Reason = dto.Reason,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow,
            Items = returnItems
        };

        await _context.PurchaseReturns.AddAsync(purchaseReturn);
        await _context.SaveChangesAsync();

        return ApiResponse<PurchaseReturnResultDto>.Ok(new PurchaseReturnResultDto
        {
            Id = purchaseReturn.Id,
            ReturnNumber = returnNo,
            SupplierId = invoice.SupplierId,
            TotalAmount = total,
            AppliedToBalance = applied,
            CashRefund = cashRefund
        });
    }
}