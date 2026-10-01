using AutoPartsERP.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Sales;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Services;

public class SalesReturnService : ISalesReturnService
{
    private readonly AppDbContext _context;

    public SalesReturnService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<SalesReturnResultDto>> CreateAsync(CreateSalesReturnDto dto, int? userId)
    {
        if (dto.Items == null || !dto.Items.Any())
            return ApiResponse<SalesReturnResultDto>.Fail("Return must contain at least one item");

        if (dto.Items.Any(i => i.Quantity <= 0))
            return ApiResponse<SalesReturnResultDto>.Fail("Quantity must be greater than zero");

        if (dto.Items.GroupBy(i => i.PartId).Any(g => g.Count() > 1))
            return ApiResponse<SalesReturnResultDto>.Fail("Each part may appear only once in a return");

        if (!await _context.Warehouses.AnyAsync(w => w.Id == dto.WarehouseId))
            return ApiResponse<SalesReturnResultDto>.Fail($"Warehouse ID {dto.WarehouseId} not found");

        var invoice = await _context.SalesInvoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == dto.SalesInvoiceId);

        if (invoice == null)
            return ApiResponse<SalesReturnResultDto>.Fail($"Sales invoice ID {dto.SalesInvoiceId} not found");

        if (invoice.Status == "Cancelled")
            return ApiResponse<SalesReturnResultDto>.Fail("Cannot return items of a cancelled invoice");

        var priorItems = await _context.SalesReturns
            .Where(r => r.SalesInvoiceId == invoice.Id && r.Status != "Cancelled")
            .SelectMany(r => r.Items)
            .Select(i => new { i.PartId, i.Quantity })
            .ToListAsync();

        var movements = await _context.StockMovements
            .Where(m => m.ReferenceNumber == invoice.InvoiceNumber && m.MovementType == "Sale")
            .ToListAsync();

        var factor = invoice.SubTotal > 0 ? invoice.TotalAmount / invoice.SubTotal : 1m;
        var returnNo = "SR-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

        var returnItems = new List<SalesReturnItem>();
        decimal total = 0;

        foreach (var it in dto.Items)
        {
            var soldRows = invoice.Items.Where(x => x.PartId == it.PartId).ToList();
            if (!soldRows.Any())
                return ApiResponse<SalesReturnResultDto>.Fail($"Part ID {it.PartId} was not sold on this invoice");

            var soldQty = soldRows.Sum(x => x.Quantity);
            var alreadyReturned = priorItems.Where(p => p.PartId == it.PartId).Sum(p => p.Quantity);
            if (alreadyReturned + it.Quantity > soldQty)
                return ApiResponse<SalesReturnResultDto>.Fail(
                    $"Part ID {it.PartId}: cannot return {it.Quantity}; sold {soldQty}, already returned {alreadyReturned}");

            var unitNet = soldRows.Sum(x => x.TotalPrice) / soldQty;
            var unitRefund = Math.Round(unitNet * factor, 2);
            var lineTotal = Math.Round(it.Quantity * unitNet * factor, 2);
            total += lineTotal;

            returnItems.Add(new SalesReturnItem
            {
                PartId = it.PartId,
                Quantity = it.Quantity,
                UnitPrice = unitRefund,
                TotalPrice = lineTotal
            });

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

            var originalCost = movements.FirstOrDefault(m => m.PartId == it.PartId)?.UnitCost
                ?? (await _context.Parts.FindAsync(it.PartId))?.CostPrice ?? 0;

            await _context.StockMovements.AddAsync(new StockMovement
            {
                PartId = it.PartId,
                WarehouseId = dto.WarehouseId,
                MovementType = "SalesReturn",
                Quantity = it.Quantity,
                UnitCost = originalCost,
                ReferenceNumber = returnNo,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        var customer = await _context.Customers.FindAsync(invoice.CustomerId);
        decimal applied = 0;
        if (customer != null && customer.CurrentBalance > 0)
        {
            applied = Math.Min(total, customer.CurrentBalance);
            customer.CurrentBalance -= applied;
        }
        var cashRefund = total - applied;

        var salesReturn = new SalesReturn
        {
            ReturnNumber = returnNo,
            SalesInvoiceId = invoice.Id,
            CustomerId = invoice.CustomerId,
            WarehouseId = dto.WarehouseId,
            ReturnDate = DateTime.UtcNow,
            TotalRefundAmount = total,
            Reason = dto.Reason,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow,
            Items = returnItems
        };

        await _context.SalesReturns.AddAsync(salesReturn);
        await _context.SaveChangesAsync();

        return ApiResponse<SalesReturnResultDto>.Ok(new SalesReturnResultDto
        {
            Id = salesReturn.Id,
            ReturnNumber = returnNo,
            CustomerId = invoice.CustomerId,
            TotalAmount = total,
            AppliedToBalance = applied,
            CashRefund = cashRefund
        });
    }
}
