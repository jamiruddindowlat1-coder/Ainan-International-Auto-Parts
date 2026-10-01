using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.Models.Accounting;
using AutoPartsERP.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class AccountingPostingServiceTests
{
    // High ids so they never collide with any seeded data.
    private const int Cash = 9001, Sales = 9002, Rent = 9003, Old = 9004, Cat = 9001;

    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Accounts.AddRange(
            new Account { Id = Cash, AccountCode = "T-1010", AccountName = "Cash", AccountType = "Asset" },
            new Account { Id = Sales, AccountCode = "T-4010", AccountName = "Sales", AccountType = "Revenue" },
            new Account { Id = Rent, AccountCode = "T-5010", AccountName = "Rent", AccountType = "Expense" },
            new Account { Id = Old, AccountCode = "T-1099", AccountName = "Old", AccountType = "Asset", IsActive = false });
        context.ExpenseCategories.Add(new ExpenseCategory { Id = Cat, Name = "Rent" });
        context.SaveChanges();
        return context;
    }

    private static PostJournalEntryDto Entry(params (int acc, decimal dr, decimal cr)[] lines) => new()
    {
        Description = "Test",
        Items = lines.Select(l => new JournalLineDto { AccountId = l.acc, Debit = l.dr, Credit = l.cr }).ToList()
    };

    private static decimal Bal(AppDbContext context, int id) => context.Accounts.Find(id)!.Balance;

    [Fact]
    public async Task Journal_WithOneLine_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context).PostJournalEntryAsync(Entry((Cash, 100m, 0m)), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Journal_Unbalanced_FailsAndChangesNothing()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 100m, 0m), (Sales, 0m, 90m)), 1);

        Assert.False(result.Success);
        Assert.Equal(0, Bal(context, Cash));
        Assert.Empty(context.JournalEntries);
    }

    [Fact]
    public async Task Journal_NegativeLine_Fails()
    {
        using var context = NewContext();

        // Totals balance (200 = 200) only because of the negative line.
        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 300m, 0m), (Rent, -100m, 0m), (Sales, 0m, 200m)), 1);

        Assert.False(result.Success);
        Assert.Empty(context.JournalEntries);
    }

    [Fact]
    public async Task Journal_LineWithBothDebitAndCredit_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 100m, 100m), (Sales, 0m, 100m)), 1);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task Journal_UnknownAccount_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 100m, 0m), (999999, 0m, 100m)), 1);

        Assert.False(result.Success);
        Assert.Equal(0, Bal(context, Cash));
    }

    [Fact]
    public async Task Journal_InactiveAccount_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 100m, 0m), (Old, 0m, 100m)), 1);

        Assert.False(result.Success);
        Assert.Equal(0, Bal(context, Cash));
    }

    [Fact]
    public async Task Journal_Balanced_PostsAndUpdatesBalancesByAccountType()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Cash, 300m, 0m), (Rent, 200m, 0m), (Sales, 0m, 500m)), 7);

        Assert.True(result.Success);
        Assert.StartsWith("JV-", result.Data!.EntryNumber);
        Assert.Equal(300, Bal(context, Cash));
        Assert.Equal(200, Bal(context, Rent));
        Assert.Equal(500, Bal(context, Sales));
        var entry = context.JournalEntries.Single();
        Assert.Equal(500, entry.TotalAmount);
        Assert.Equal(7, entry.CreatedByUserId);
        Assert.Equal(3, context.JournalItems.Count());
    }

    [Fact]
    public async Task Journal_CreditToAsset_ReducesItsBalance()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .PostJournalEntryAsync(Entry((Rent, 200m, 0m), (Cash, 0m, 200m)), 1);

        Assert.True(result.Success);
        Assert.Equal(-200, Bal(context, Cash));
        Assert.Equal(200, Bal(context, Rent));
    }

    [Fact]
    public async Task Expense_ZeroAmount_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .RecordExpenseAsync(new RecordExpenseDto { ExpenseCategoryId = Cat, Amount = 0 }, 1);

        Assert.False(result.Success);
        Assert.Empty(context.Expenses);
    }

    [Fact]
    public async Task Expense_UnknownCategory_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .RecordExpenseAsync(new RecordExpenseDto { ExpenseCategoryId = 999999, Amount = 100 }, 1);

        Assert.False(result.Success);
        Assert.Empty(context.Expenses);
    }

    [Fact]
    public async Task Expense_UnknownAccount_Fails()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context)
            .RecordExpenseAsync(new RecordExpenseDto { ExpenseCategoryId = Cat, AccountId = 999999, Amount = 100 }, 1);

        Assert.False(result.Success);
        Assert.Empty(context.Expenses);
    }

    [Fact]
    public async Task Expense_Valid_SavesWithGeneratedNumberUserAndDefaultPaymentMethod()
    {
        using var context = NewContext();

        var result = await new AccountingPostingService(context).RecordExpenseAsync(
            new RecordExpenseDto { ExpenseCategoryId = Cat, Amount = 150, PaymentMethod = "", Reference = "R1" }, 5);

        Assert.True(result.Success);
        var expense = context.Expenses.Single();
        Assert.StartsWith("EXP-", expense.ExpenseNumber);
        Assert.Equal(150, expense.Amount);
        Assert.Equal("Cash", expense.PaymentMethod);
        Assert.Equal(5, expense.CreatedByUserId);
    }
}