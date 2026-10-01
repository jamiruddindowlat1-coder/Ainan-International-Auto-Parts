using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class PurchaseServiceTests
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
        context.WarehouseStocks.Add(new WarehouseStock { WarehouseId = 1, PartId = 1, Quantity = 10 });
        context.SaveChanges();
        return context;
    }

    private static CreatePurchaseInvoiceDto NewDto(int qty, decimal unitPrice, decimal paid, int partId = 1) => new()
    {
        SupplierId = 1,
        WarehouseId = 1,
        PaidAmount = paid,
        Items = new List<PurchaseItemInputDto>
        {
            new() { PartId = partId, Quantity = qty, UnitPrice = unitPrice }
        }
    };

    [Fact]
    public async Task CreatePurchase_WithNoItems_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 70, 0);
        dto.Items = new List<PurchaseItemInputDto>();

        var result = await new PurchaseService(context).CreatePurchaseAsync(dto, 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreatePurchase_WithUnknownPart_Fails()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(1, 70, 0, 999), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreatePurchase_IncreasesStock_UpdatesCostPrice_LogsMovement()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(5, 70, 350), 1);

        Assert.True(result.Success);
        Assert.Equal(15, context.WarehouseStocks.Single(s => s.PartId == 1).Quantity);
        Assert.Equal(70, context.Parts.Single(p => p.Id == 1).CostPrice);
        Assert.Single(context.StockMovements);
        Assert.Equal(350, context.PurchaseInvoices.Single().TotalAmount);
    }

    [Fact]
    public async Task CreatePurchase_NoExistingStock_CreatesStockRecord()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(4, 30, 120, 2), 1);

        Assert.True(result.Success);
        Assert.Equal(2, context.WarehouseStocks.Count());
        Assert.Equal(4, context.WarehouseStocks.Single(s => s.PartId == 2).Quantity);
    }

    [Fact]
    public async Task CreatePurchase_PartialPayment_SetsPartialAndRaisesSupplierBalance()
    {
        using var context = NewContext();

        await new PurchaseService(context).CreatePurchaseAsync(NewDto(5, 70, 100), 1);

        var invoice = context.PurchaseInvoices.Single();
        Assert.Equal("Partial", invoice.PaymentStatus);
        Assert.Equal(250, invoice.DueAmount);
        Assert.Equal(250, context.Suppliers.Single().CurrentBalance);
    }

    [Fact]
    public async Task CreatePurchase_FullPayment_SetsPaidAndKeepsBalance()
    {
        using var context = NewContext();

        await new PurchaseService(context).CreatePurchaseAsync(NewDto(5, 70, 350), 1);

        Assert.Equal("Paid", context.PurchaseInvoices.Single().PaymentStatus);
        Assert.Equal(0, context.Suppliers.Single().CurrentBalance);
    }

    [Fact]
    public async Task CreatePurchase_ZeroQuantity_Fails()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(0, 70, 0), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreatePurchase_NegativeQuantity_FailsAndKeepsStock()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(-5, 70, 0), 1);

        Assert.False(result.Success);
        Assert.Equal(10, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task CreatePurchase_ZeroUnitPrice_Fails()
    {
        using var context = NewContext();

        var result = await new PurchaseService(context).CreatePurchaseAsync(NewDto(1, 0, 0), 1);

        Assert.False(result.Success);
        Assert.Equal(60, context.Parts.Single(p => p.Id == 1).CostPrice);
    }

    [Fact]
    public async Task CreatePurchase_UnknownSupplier_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 70, 0);
        dto.SupplierId = 999;

        var result = await new PurchaseService(context).CreatePurchaseAsync(dto, 1);

        Assert.False(result.Success);
        Assert.Empty(context.PurchaseInvoices);
    }

    [Fact]
    public async Task CreatePurchase_UnknownWarehouse_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 70, 0);
        dto.WarehouseId = 999;

        var result = await new PurchaseService(context).CreatePurchaseAsync(dto, 1);

        Assert.False(result.Success);
        Assert.Equal(1, context.WarehouseStocks.Count());
    }

    [Fact]
    public async Task CreatePurchase_DiscountExceedsTotal_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 70, 0);
        dto.DiscountAmount = 5000;

        var result = await new PurchaseService(context).CreatePurchaseAsync(dto, 1);

        Assert.False(result.Success);
        Assert.Empty(context.PurchaseInvoices);
    }

    [Fact]
    public async Task CreatePurchase_NegativeShippingCost_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 70, 0);
        dto.ShippingCost = -10;

        var result = await new PurchaseService(context).CreatePurchaseAsync(dto, 1);

        Assert.False(result.Success);
    }
}
