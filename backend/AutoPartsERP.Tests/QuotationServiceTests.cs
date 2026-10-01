using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class QuotationServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

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
        context.SaveChanges();
        return context;
    }

    private static CreateQuotationDto Dto(params (int part, int qty, decimal? price)[] lines) => new()
    {
        CustomerId = 1,
        Items = lines.Select(l => new QuotationItemInputDto { PartId = l.part, Quantity = l.qty, UnitPrice = l.price }).ToList()
    };

    [Fact]
    public async Task Quotation_WithNoItems_Fails()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto());

        Assert.False(result.Success);
        Assert.Empty(context.Quotations);
    }

    [Fact]
    public async Task Quotation_ZeroQuantity_Fails()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto((1, 0, null)));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Quotation_DuplicatePart_Fails()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto((1, 1, null), (1, 2, null)));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Quotation_UnknownCustomer_Fails()
    {
        using var context = NewContext();
        var dto = Dto((1, 1, null));
        dto.CustomerId = 999;

        var result = await new QuotationService(context).CreateAsync(dto);

        Assert.False(result.Success);
        Assert.Empty(context.Quotations);
    }

    [Fact]
    public async Task Quotation_UnknownPart_Fails()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto((999, 1, null)));

        Assert.False(result.Success);
        Assert.Empty(context.Quotations);
    }

    [Fact]
    public async Task Quotation_NegativeUnitPrice_Fails()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto((1, 1, -5m)));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Quotation_NegativeTax_Fails()
    {
        using var context = NewContext();
        var dto = Dto((1, 1, null));
        dto.TaxAmount = -10;

        var result = await new QuotationService(context).CreateAsync(dto);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Quotation_DiscountAboveSubtotal_Fails()
    {
        using var context = NewContext();
        var dto = Dto((1, 1, null));   // subtotal 100
        dto.DiscountAmount = 150;

        var result = await new QuotationService(context).CreateAsync(dto);

        Assert.False(result.Success);
        Assert.Empty(context.Quotations);
    }

    [Fact]
    public async Task Quotation_UsesSellingPrice_AndCalculatesTotalOnServer()
    {
        using var context = NewContext();
        var dto = Dto((1, 3, null));   // 3 x 100 = 300
        dto.DiscountAmount = 20;
        dto.TaxAmount = 10;

        var result = await new QuotationService(context).CreateAsync(dto);

        Assert.True(result.Success);
        Assert.Equal(300, result.Data!.SubTotal);
        Assert.Equal(290, result.Data.TotalAmount);   // 300 - 20 + 10
        var saved = context.Quotations.Include(q => q.Items).Single();
        Assert.StartsWith("QT-", saved.QuotationNumber);
        Assert.Equal("Pending", saved.Status);
        Assert.Equal(290, saved.TotalAmount);
        Assert.Single(saved.Items);
        Assert.Equal(100, saved.Items.Single().UnitPrice);
        Assert.Equal(300, saved.Items.Single().TotalPrice);
        Assert.NotNull(saved.ExpiryDate);
    }

    [Fact]
    public async Task Quotation_CustomUnitPrice_OverridesSellingPrice_WithMultipleItems()
    {
        using var context = NewContext();

        var result = await new QuotationService(context).CreateAsync(Dto((1, 1, null), (2, 2, 45m)));   // 100 + 90

        Assert.True(result.Success);
        Assert.Equal(190, result.Data!.TotalAmount);
        var saved = context.Quotations.Include(q => q.Items).Single();
        Assert.Equal(2, saved.Items.Count);
        Assert.Equal(45, saved.Items.Single(i => i.PartId == 2).UnitPrice);
    }
}