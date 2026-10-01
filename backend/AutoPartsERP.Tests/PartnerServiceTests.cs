using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class PartnerServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Customers.Add(new Customer { Id = 1, Name = "Old Customer", Phone = "0111", CurrentBalance = 500 });
        context.Customers.Add(new Customer { Id = 2, Name = "Clear Customer", Phone = "0222", CurrentBalance = 0 });
        context.Suppliers.Add(new Supplier { Id = 1, Name = "Old Supplier", Phone = "0333", CurrentBalance = 800 });
        context.SaveChanges();
        return context;
    }

    private static CustomerDto Cust() => new()
    {
        Name = "New Customer", Phone = "0100", CustomerType = "Retail", CreditLimit = 1000, IsActive = true
    };

    private static SupplierDto Supp() => new()
    {
        Name = "New Supplier", Phone = "0200", IsActive = true
    };

    [Fact]
    public async Task Customer_Create_EmptyName_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.Name = "  ";

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.False(result.Success);
        Assert.Equal(2, context.Customers.Count());
    }

    [Fact]
    public async Task Customer_Create_EmptyPhone_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.Phone = "";

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Customer_Create_NegativeCreditLimit_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.CreditLimit = -1;

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Customer_Create_UnknownType_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.CustomerType = "VIP";

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Customer_Create_InvalidEmail_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.Email = "not an email";

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Customer_Create_Valid_TrimsAndIgnoresClientBalance()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.Name = "  Rahim Garage  ";
        dto.CustomerType = "garage";
        dto.CurrentBalance = 999;
        dto.IsActive = false;

        var result = await new PartnerService(context).CreateCustomerAsync(dto);

        Assert.True(result.Success);
        Assert.Equal(0, result.Data!.CurrentBalance);
        Assert.True(result.Data.IsActive);
        var saved = context.Customers.Single(c => c.Name == "Rahim Garage");
        Assert.Equal("Garage", saved.CustomerType);
        Assert.Equal(0, saved.CurrentBalance);
        Assert.True(saved.IsActive);
    }

    [Fact]
    public async Task Customer_Update_Unknown_Fails()
    {
        using var context = NewContext();

        var result = await new PartnerService(context).UpdateCustomerAsync(999, Cust());

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Customer_Update_ChangesFields_ButNotBalance()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.Name = "Renamed";
        dto.CurrentBalance = 0;   // client tries to wipe the balance

        var result = await new PartnerService(context).UpdateCustomerAsync(1, dto);

        Assert.True(result.Success);
        var saved = context.Customers.Single(c => c.Id == 1);
        Assert.Equal("Renamed", saved.Name);
        Assert.Equal(500, saved.CurrentBalance);
        Assert.Equal(500, result.Data!.CurrentBalance);
    }

    [Fact]
    public async Task Customer_Deactivate_WithBalance_Fails()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.IsActive = false;

        var result = await new PartnerService(context).UpdateCustomerAsync(1, dto);

        Assert.False(result.Success);
        Assert.True(context.Customers.Single(c => c.Id == 1).IsActive);
    }

    [Fact]
    public async Task Customer_Deactivate_WithoutBalance_Works()
    {
        using var context = NewContext();
        var dto = Cust();
        dto.IsActive = false;

        var result = await new PartnerService(context).UpdateCustomerAsync(2, dto);

        Assert.True(result.Success);
        Assert.False(context.Customers.Single(c => c.Id == 2).IsActive);
    }

    [Fact]
    public async Task Supplier_Create_EmptyName_Fails()
    {
        using var context = NewContext();
        var dto = Supp();
        dto.Name = "";

        var result = await new PartnerService(context).CreateSupplierAsync(dto);

        Assert.False(result.Success);
        Assert.Single(context.Suppliers);
    }

    [Fact]
    public async Task Supplier_Create_Valid_IgnoresClientBalance()
    {
        using var context = NewContext();
        var dto = Supp();
        dto.CurrentBalance = 999;

        var result = await new PartnerService(context).CreateSupplierAsync(dto);

        Assert.True(result.Success);
        Assert.Equal(0, result.Data!.CurrentBalance);
        Assert.Equal(0, context.Suppliers.Single(s => s.Name == "New Supplier").CurrentBalance);
    }

    [Fact]
    public async Task Supplier_Update_Unknown_Fails()
    {
        using var context = NewContext();

        var result = await new PartnerService(context).UpdateSupplierAsync(999, Supp());

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Supplier_Update_ChangesFields_ButNotBalance()
    {
        using var context = NewContext();
        var dto = Supp();
        dto.Name = "Renamed Supplier";
        dto.CurrentBalance = 0;

        var result = await new PartnerService(context).UpdateSupplierAsync(1, dto);

        Assert.True(result.Success);
        var saved = context.Suppliers.Single(s => s.Id == 1);
        Assert.Equal("Renamed Supplier", saved.Name);
        Assert.Equal(800, saved.CurrentBalance);
    }

    [Fact]
    public async Task Supplier_Deactivate_WithBalance_Fails()
    {
        using var context = NewContext();
        var dto = Supp();
        dto.IsActive = false;

        var result = await new PartnerService(context).UpdateSupplierAsync(1, dto);

        Assert.False(result.Success);
        Assert.True(context.Suppliers.Single(s => s.Id == 1).IsActive);
    }
}