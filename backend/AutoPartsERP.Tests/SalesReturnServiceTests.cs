using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class SalesReturnServiceTests
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
        context.Parts.Add(new Part
        {
            Id = 2, CostPrice = 30, SellingPrice = 50,
            Category = new Category(), Brand = new Brand(), Unit = new Unit()
        });
        context.WarehouseStocks.Add(new WarehouseStock { WarehouseId = 1, PartId = 1, Quantity = 10 });
        context.SaveChanges();
        return context;
    }

    // Sells 4 units at 100 with 10% discount = 360 total. Stock goes 10 -> 6.
    private static async Task<int> SellAsync(AppDbContext context, decimal paid)
    {
        var dto = new CreateSalesInvoiceDto
        {
            CustomerId = 1,
            WarehouseId = 1,
            PaidAmount = paid,
            Items = new List<SalesItemInputDto>
            {
                new() { PartId = 1, Quantity = 4, UnitPrice = 100, DiscountPercent = 10 }
            }
        };
        await new SalesService(context).CreateSaleAsync(dto, 1);
        return context.SalesInvoices.Single().Id;
    }

    private static CreateSalesReturnDto ReturnDto(int invoiceId, int qty, int partId = 1, int warehouseId = 1) => new()
    {
        SalesInvoiceId = invoiceId,
        WarehouseId = warehouseId,
        Reason = "Defective",
        Items = new List<SalesReturnItemInputDto> { new() { PartId = partId, Quantity = qty } }
    };

    [Fact]
    public async Task Return_WithNoItems_Fails()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);
        var dto = ReturnDto(id, 1);
        dto.Items = new List<SalesReturnItemInputDto>();

        var result = await new SalesReturnService(context).CreateAsync(dto, 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_ZeroQuantity_Fails()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 0), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_UnknownInvoice_Fails()
    {
        using var context = NewContext();
        await SellAsync(context, 0);

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(999, 1), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_UnknownWarehouse_Fails()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 1, 1, 999), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_PartNotOnInvoice_Fails()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 1, 2), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Return_MoreThanSold_FailsAndKeepsStock()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 5), 1);

        Assert.False(result.Success);
        Assert.Equal(6, context.WarehouseStocks.Single().Quantity);
        Assert.Empty(context.SalesReturns);
    }

    [Fact]
    public async Task Return_CumulativeOverSold_Fails()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);
        var service = new SalesReturnService(context);

        var first = await service.CreateAsync(ReturnDto(id, 3), 1);
        var second = await service.CreateAsync(ReturnDto(id, 2), 1);

        Assert.True(first.Success);
        Assert.False(second.Success);
        Assert.Equal(9, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task Return_OnCreditSale_RaisesStock_LogsMovement_ReducesBalance()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);   // balance 360, stock 6

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 1), 1);

        Assert.True(result.Success);
        Assert.Equal(7, context.WarehouseStocks.Single().Quantity);
        Assert.Equal(270, context.Customers.Single().CurrentBalance);   // 360 - 90
        Assert.Equal(90, context.SalesReturns.Single().TotalRefundAmount);
        Assert.Single(context.SalesReturnItems);
        var movement = context.StockMovements.Single(m => m.MovementType == "SalesReturn");
        Assert.Equal(1, movement.Quantity);
        Assert.Equal(60, movement.UnitCost);
    }

    [Fact]
    public async Task Return_OnPaidSale_GivesCashRefund_AndKeepsBalanceAtZero()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 360);   // fully paid, balance 0

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 1), 1);

        Assert.True(result.Success);
        Assert.Equal(0, context.Customers.Single().CurrentBalance);
        Assert.Equal(7, context.WarehouseStocks.Single().Quantity);
    }

    [Fact]
    public async Task Return_ExceedingBalance_NeverMakesBalanceNegative()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 300);   // balance 60
        var service = new SalesReturnService(context);

        var result = await service.CreateAsync(ReturnDto(id, 1), 1);   // refund 90

        Assert.True(result.Success);
        Assert.Equal(0, context.Customers.Single().CurrentBalance);
    }

    [Fact]
    public async Task Return_InactiveWarehouse_FailsAndKeepsStockAndBalance()
    {
        using var context = NewContext();
        var id = await SellAsync(context, 0);   // balance 360, stock 6
        context.Warehouses.Single(w => w.Id == 1).IsActive = false;
        context.SaveChanges();

        var result = await new SalesReturnService(context).CreateAsync(ReturnDto(id, 1), 1);

        Assert.False(result.Success);
        Assert.Equal(6, context.WarehouseStocks.Single().Quantity);
        Assert.Equal(360, context.Customers.Single().CurrentBalance);
        Assert.Empty(context.SalesReturns);
    }}
