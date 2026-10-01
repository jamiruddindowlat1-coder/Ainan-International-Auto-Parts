using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class PurchaseReturnServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Warehouses.Add(new Warehouse { Id = 1 });
        context.Suppliers.Add(new Supplier { Id = 1, Name = "Test Supplier", Phone = "0100000000" });
        context.Parts.Add(new Part
        {
            Id = 1, CostPrice = 60, SellingPrice = 100,
            Category = new Category(), Brand = new Brand(), Unit = new Unit()
        });
        context.Parts.Add(new Part
        {
            Id = 2, CostPrice = 30, SellingPrice = 50,
            Category = new Category(), Brand = new Brand(), Unit = new Unit()
        });
        context.SaveChanges();
        return context;
    }

    // Buys 5 units at 70 = 350. Stock 0 -> 5.
    private static async Task<int> BuyAsync(AppDbContext context, decimal paid)
    {
        var dto = new CreatePurchaseInvoiceDto
        {
            SupplierId = 1,
            WarehouseId = 1,
            PaidAmount = paid,
            Items = new List<PurchaseItemInputDto>
            {
                new() { PartId = 1, Quantity = 5, UnitPrice = 70 }
            }
        };
        await new PurchaseService(context).CreatePurchaseAsync(dto, 1);
        return context.PurchaseInvoices.Single().Id;
    }

    private static CreatePurchaseReturnDto ReturnDto(int invoiceId, int qty, int partId = 1, int warehouseId = 1) => new()
    {
        PurchaseInvoiceId = invoiceId,
        WarehouseId = warehouseId,
        Reason = "Defective",
        Items = new List<PurchaseReturnItemInputDto> { new() { PartId = partId, Quantity = qty } }
    };

    [Fact]
    public async Task Return_WithNoItems_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);
        var dto = ReturnDto(id, 1);
        dto.Items = new List<PurchaseReturnItemInputDto>();

        var result = await new PurchaseReturnService(context).CreateAsync(dto, 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_ZeroQuantity_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 0), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_UnknownInvoice_Fails()
    {
        using var context = NewContext();
        await BuyAsync(context, 0);

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(999, 1), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_UnknownWarehouse_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 1, 1, 999), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_PartNotOnInvoice_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 1, 2), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_MoreThanBought_FailsAndKeepsStock()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 6), 1);

        Assert.False(result.Success);
        Assert.Equal(5, context.WarehouseStocks.Single().Quantity);
        Assert.Empty(context.PurchaseReturns);
    }

    [Fact]
    public async Task Return_CumulativeOverBought_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);
        var service = new PurchaseReturnService(context);

        var first = await service.CreateAsync(ReturnDto(id, 3), 1);
        var second = await service.CreateAsync(ReturnDto(id, 3), 1);

        Assert.True(first.Success);
        Assert.False(second.Success);
        Assert.Equal(2, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task Return_WhenStockAlreadySold_Fails()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);
        context.WarehouseStocks.Single().Quantity = 1;   // 4 of the 5 already sold
        context.SaveChanges();

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 2), 1);

        Assert.False(result.Success);
        Assert.Equal(1, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task Return_OnCreditPurchase_ReducesStock_LogsMovement_ReducesBalance()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 0);   // balance 350, stock 5

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 2), 1);

        Assert.True(result.Success);
        Assert.Equal(3, context.WarehouseStocks.Single().Quantity);
        Assert.Equal(210, context.Suppliers.Single().CurrentBalance);   // 350 - 140
        Assert.Equal(140, context.PurchaseReturns.Single().TotalRefundAmount);
        Assert.Single(context.PurchaseReturnItems);
        var movement = context.StockMovements.Single(m => m.MovementType == "PurchaseReturn");
        Assert.Equal(-2, movement.Quantity);
        Assert.Equal(70, movement.UnitCost);
    }

    [Fact]
    public async Task Return_OnPaidPurchase_GivesCashRefund_AndKeepsBalanceAtZero()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 350);   // fully paid

        var result = await new PurchaseReturnService(context).CreateAsync(ReturnDto(id, 2), 1);

        Assert.True(result.Success);
        Assert.Equal(0, context.Suppliers.Single().CurrentBalance);
        Assert.Equal(140, result.Data!.CashRefund);
        Assert.Equal(3, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task Return_ExceedingBalance_NeverMakesBalanceNegative()
    {
        using var context = NewContext();
        var id = await BuyAsync(context, 300);   // balance 50
        var service = new PurchaseReturnService(context);

        var result = await service.CreateAsync(ReturnDto(id, 2), 1);   // refund 140

        Assert.True(result.Success);
        Assert.Equal(0, context.Suppliers.Single().CurrentBalance);
        Assert.Equal(90, result.Data!.CashRefund);
    }
}