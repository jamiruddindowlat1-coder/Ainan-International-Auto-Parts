using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class ExpenseServiceTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateExpense_WithoutNumber_GeneratesExpPrefix()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new ExpenseDto { ExpenseCategoryId = 1, Amount = 300, Note = "Test" };

        var result = await service.CreateExpenseAsync(dto);

        Assert.True(result.Success);
        Assert.StartsWith("EXP-", context.Expenses.Single().ExpenseNumber);
    }

    [Fact]
    public async Task CreateExpense_WithNumber_KeepsGivenNumber()
    {
        using var context = NewContext();
        var service = new AccountingService(context);
        var dto = new ExpenseDto { ExpenseNumber = "EXP-TEST-1", ExpenseCategoryId = 1, Amount = 120, Note = "Test" };

        await service.CreateExpenseAsync(dto);

        Assert.Equal("EXP-TEST-1", context.Expenses.Single().ExpenseNumber);
    }
}
