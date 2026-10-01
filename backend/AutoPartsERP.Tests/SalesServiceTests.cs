using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class SalesServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Warehouses.Add(new Warehouse { Id = 1 });
        context.Customers.Add(new Customer { Id = 1, Name = "Test Customer", Phone = "0100000000" });
        context.Parts.Add(new Part
        {
            Id = 1, CostPrice = 60, SellingPrice = 100,
            Category = new Category(), Brand = new Brand(), Unit = new Unit()
        });
        context.WarehouseStocks.Add(new WarehouseStock { WarehouseId = 1, PartId = 1, Quantity = 10 });
        context.SaveChanges();
        return context;
    }

    private static CreateSalesInvoiceDto NewDto(int qty, decimal discountPercent, decimal paid) => new()
    {
        CustomerId = 1,
        WarehouseId = 1,
        PaidAmount = paid,
        Items = new List<SalesItemInputDto>
        {
            new() { PartId = 1, Quantity = qty, UnitPrice = 100, DiscountPercent = discountPercent }
        }
    };

    [Fact]
    public async Task CreateSale_WithNoItems_Fails()
    {
        using var context = NewContext();
        var service = new SalesService(context);
        var dto = NewDto(1, 0, 0);
        dto.Items = new List<SalesItemInputDto>();

        var result = await service.CreateSaleAsync(dto, 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateSale_WithUnknownPart_Fails()
    {
        using var context = NewContext();
        var service = new SalesService(context);
        var dto = NewDto(1, 0, 0);
        dto.Items[0].PartId = 999;

        var result = await service.CreateSaleAsync(dto, 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateSale_ReducesStock_LogsMovement_AndAppliesDiscount()
    {
        using var context = NewContext();
        var service = new SalesService(context);

        var result = await service.CreateSaleAsync(NewDto(2, 10, 180), 1);

        Assert.True(result.Success);
        Assert.Equal(8, context.WarehouseStocks.Single().Quantity);
        Assert.Single(context.StockMovements);
        Assert.Equal(180, context.SalesInvoices.Single().TotalAmount);
    }

    [Fact]
    public async Task CreateSale_PartialPayment_SetsPartialAndRaisesCustomerBalance()
    {
        using var context = NewContext();
        var service = new SalesService(context);

        await service.CreateSaleAsync(NewDto(2, 10, 100), 1);

        var invoice = context.SalesInvoices.Single();
        Assert.Equal("Partial", invoice.PaymentStatus);
        Assert.Equal(80, invoice.DueAmount);
        Assert.Equal(80, context.Customers.Single().CurrentBalance);
    }

    [Fact]
    public async Task CreateSale_FullPayment_SetsPaidAndKeepsBalance()
    {
        using var context = NewContext();
        var service = new SalesService(context);

        await service.CreateSaleAsync(NewDto(2, 10, 180), 1);

        Assert.Equal("Paid", context.SalesInvoices.Single().PaymentStatus);
        Assert.Equal(0, context.Customers.Single().CurrentBalance);
    }

    [Fact]
    public async Task CreateSale_MoreThanAvailableStock_Fails()
    {
        using var context = NewContext();
        var service = new SalesService(context);

        var result = await service.CreateSaleAsync(NewDto(50, 0, 0), 1);

        Assert.False(result.Success);
        Assert.Equal(10, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task CreateSale_ZeroQuantity_Fails()
    {
        using var context = NewContext();
        var result = await new SalesService(context).CreateSaleAsync(NewDto(0, 0, 0), 1);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateSale_DiscountOver100_Fails()
    {
        using var context = NewContext();
        var result = await new SalesService(context).CreateSaleAsync(NewDto(1, 150, 0), 1);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateSale_UnknownCustomer_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 0, 0);
        dto.CustomerId = 999;
        var result = await new SalesService(context).CreateSaleAsync(dto, 1);
        Assert.False(result.Success);
        Assert.Empty(context.SalesInvoices);
    }

    [Fact]
    public async Task CreateSale_UnknownWarehouse_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 0, 0);
        dto.WarehouseId = 999;
        var result = await new SalesService(context).CreateSaleAsync(dto, 1);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task CreateSale_DiscountAmountExceedsTotal_Fails()
    {
        using var context = NewContext();
        var dto = NewDto(1, 0, 0);
        dto.DiscountAmount = 5000;
        var result = await new SalesService(context).CreateSaleAsync(dto, 1);
        Assert.False(result.Success);
        Assert.Empty(context.SalesInvoices);
    }
}
