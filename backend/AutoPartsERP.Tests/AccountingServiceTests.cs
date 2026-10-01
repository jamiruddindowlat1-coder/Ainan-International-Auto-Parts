using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class AccountingServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateIncome_WithoutNumber_GeneratesIncPrefix()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new IncomeDto { Source = "Sales", Amount = 1500 };

        var result = await service.CreateIncomeAsync(dto);

        Assert.True(result.Success);
        Assert.StartsWith("INC-", context.Incomes.Single().IncomeNumber);
    }

    [Fact]
    public async Task CreateIncome_WithNumber_KeepsGivenNumber()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new IncomeDto { IncomeNumber = "INC-TEST-1", Source = "Sales", Amount = 200 };

        await service.CreateIncomeAsync(dto);

        Assert.Equal("INC-TEST-1", context.Incomes.Single().IncomeNumber);
    }

    [Fact]
    public async Task CreateIncome_SavesRecordToDatabase()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new IncomeDto { Source = "Service Fee", Amount = 750 };

        var result = await service.CreateIncomeAsync(dto);

        var saved = context.Incomes.Single();
        Assert.Equal("Service Fee", saved.Source);
        Assert.True(saved.Amount == 750);
        Assert.Equal(saved.Id, result.Data!.Id);
    }

    [Fact]
    public async Task CreateIncome_WithoutDate_UsesCurrentDate()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new IncomeDto { Source = "Sales", Amount = 100 };

        await service.CreateIncomeAsync(dto);

        var saved = context.Incomes.Single();
        Assert.True(saved.IncomeDate > DateTime.UtcNow.AddMinutes(-1));
    }
}
