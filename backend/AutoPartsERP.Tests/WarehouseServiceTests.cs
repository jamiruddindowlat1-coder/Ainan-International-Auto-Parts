using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class WarehouseServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Warehouses.Add(new Warehouse { Id = 1, Name = "Main", Code = "MAIN", IsDefault = true });
        context.Warehouses.Add(new Warehouse { Id = 2, Name = "Stocked", Code = "WH2" });
        context.Warehouses.Add(new Warehouse { Id = 3, Name = "Empty", Code = "WH3" });
        context.Parts.Add(new Part
        {
            Id = 1, CostPrice = 60, SellingPrice = 100,
            Category = new Category(), Brand = new Brand(), Unit = new Unit()
        });
        context.WarehouseStocks.Add(new WarehouseStock { WarehouseId = 2, PartId = 1, Quantity = 5 });
        context.WarehouseStocks.Add(new WarehouseStock { WarehouseId = 3, PartId = 1, Quantity = 0 });
        context.SaveChanges();
        return context;
    }

    private static WarehouseDto Dto(string name = "New", string code = "NEW1", bool active = true) => new()
    {
        Name = name, Code = code, Location = "Dhaka", IsActive = active
    };

    [Fact]
    public async Task Create_EmptyName_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).CreateAsync(Dto(name: "  "));

        Assert.False(result.Success);
        Assert.Equal(3, context.Warehouses.Count());
    }

    [Fact]
    public async Task Create_EmptyCode_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).CreateAsync(Dto(code: ""));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Create_DuplicateCode_IgnoringCase_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).CreateAsync(Dto(code: "main"));

        Assert.False(result.Success);
        Assert.Equal(3, context.Warehouses.Count());
    }

    [Fact]
    public async Task Create_Valid_TrimsAndSaves()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).CreateAsync(Dto(name: "  North Store  ", code: " NS1 "));

        Assert.True(result.Success);
        var saved = context.Warehouses.Single(w => w.Code == "NS1");
        Assert.Equal("North Store", saved.Name);
        Assert.False(saved.IsDefault);
        Assert.Equal(saved.Id, result.Data!.Id);
    }

    [Fact]
    public async Task Update_Unknown_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(999, Dto());

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Update_CodeTakenByAnotherWarehouse_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(3, Dto(code: "WH2"));

        Assert.False(result.Success);
        Assert.Equal("WH3", context.Warehouses.Single(w => w.Id == 3).Code);
    }

    [Fact]
    public async Task Update_KeepingOwnCode_ChangesFields()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(3, Dto(name: "Renamed", code: "wh3"));

        Assert.True(result.Success);
        var saved = context.Warehouses.Single(w => w.Id == 3);
        Assert.Equal("Renamed", saved.Name);
        Assert.Equal("wh3", saved.Code);
        Assert.Equal("Dhaka", saved.Location);
    }

    [Fact]
    public async Task Deactivate_WithStock_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(2, Dto(code: "WH2", active: false));

        Assert.False(result.Success);
        Assert.True(context.Warehouses.Single(w => w.Id == 2).IsActive);
    }

    [Fact]
    public async Task Deactivate_WarehouseWithNoStock_Works()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(3, Dto(code: "WH3", active: false));

        Assert.True(result.Success);
        Assert.False(context.Warehouses.Single(w => w.Id == 3).IsActive);
    }

    [Fact]
    public async Task Deactivate_DefaultWarehouse_Fails()
    {
        using var context = NewContext();

        var result = await new WarehouseService(context).UpdateAsync(1, Dto(code: "MAIN", active: false));

        Assert.False(result.Success);
        Assert.True(context.Warehouses.Single(w => w.Id == 1).IsActive);
    }
}