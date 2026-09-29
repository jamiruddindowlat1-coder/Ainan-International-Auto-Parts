using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Accounting;

namespace AutoPartsERP.API.Services;

public class AccountingService : IAccountingService
{
    private readonly AppDbContext _context;

    public AccountingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<IncomeDto>>> GetIncomesAsync()
    {
        var list = await _context.Incomes.AsNoTracking()
            .Select(i => new IncomeDto
            {
                Id = i.Id,
                IncomeNumber = i.IncomeNumber,
                Source = i.Source,
                Amount = i.Amount,
                IncomeDate = i.IncomeDate,
                Note = i.Note
            }).ToListAsync();
        return ApiResponse<List<IncomeDto>>.Ok(list);
    }

    public async Task<ApiResponse<IncomeDto>> CreateIncomeAsync(IncomeDto dto)
    {
        var income = new Income
        {
            IncomeNumber = dto.IncomeNumber ?? "INC-" + DateTime.Now.Ticks,
            Source = dto.Source,
            Amount = dto.Amount,
            IncomeDate = dto.IncomeDate == default ? DateTime.UtcNow : dto.IncomeDate,
            Note = dto.Note
        };
        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync();
        dto.Id = income.Id;
        return ApiResponse<IncomeDto>.Ok(dto);
    }

    public async Task<ApiResponse<List<ExpenseDto>>> GetExpensesAsync()
    {
        var list = await _context.Expenses.AsNoTracking()
            .Select(e => new ExpenseDto
            {
                Id = e.Id,
                ExpenseNumber = e.ExpenseNumber,
                ExpenseCategoryId = e.ExpenseCategoryId,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate,
                Note = e.Note
            }).ToListAsync();
        return ApiResponse<List<ExpenseDto>>.Ok(list);
    }

    public async Task<ApiResponse<ExpenseDto>> CreateExpenseAsync(ExpenseDto dto)
    {
        var expense = new Expense
        {
            ExpenseNumber = dto.ExpenseNumber ?? "EXP-" + DateTime.Now.Ticks,
            ExpenseCategoryId = dto.ExpenseCategoryId,
            Amount = dto.Amount,
            ExpenseDate = dto.ExpenseDate == default ? DateTime.UtcNow : dto.ExpenseDate,
            Note = dto.Note
        };
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        dto.Id = expense.Id;
        return ApiResponse<ExpenseDto>.Ok(dto);
    }

    public async Task<ApiResponse<List<LiabilityDto>>> GetLiabilitiesAsync()
    {
        var list = await _context.Liabilities.AsNoTracking()
            .Select(l => new LiabilityDto
            {
                Id = l.Id,
                Title = l.Title,
                CreditorName = l.CreditorName,
                Amount = l.Amount,
                PaidAmount = l.PaidAmount,
                Status = l.Status
            }).ToListAsync();
        return ApiResponse<List<LiabilityDto>>.Ok(list);
    }

    public async Task<ApiResponse<LiabilityDto>> CreateLiabilityAsync(LiabilityDto dto)
    {
        var liability = new Liability
        {
            Title = dto.Title,
            CreditorName = dto.CreditorName,
            Amount = dto.Amount,
            PaidAmount = dto.PaidAmount,
            Status = dto.Status ?? "Active",
            CreatedAt = DateTime.UtcNow
        };
        await _context.Liabilities.AddAsync(liability);
        await _context.SaveChangesAsync();
        dto.Id = liability.Id;
        return ApiResponse<LiabilityDto>.Ok(dto);
    }

    public async Task<ApiResponse<List<AssetDto>>> GetAssetsAsync()
    {
        var list = await _context.Assets.AsNoTracking()
            .Select(a => new AssetDto
            {
                Id = a.Id,
                AssetName = a.AssetName,
                AssetCode = a.AssetCode,
                CurrentValue = a.CurrentValue,
                Description = a.Description
            }).ToListAsync();
        return ApiResponse<List<AssetDto>>.Ok(list);
    }

    public async Task<ApiResponse<AssetDto>> CreateAssetAsync(AssetDto dto)
    {
        var asset = new Asset
        {
            AssetName = dto.AssetName,
            AssetCode = dto.AssetCode,
            CurrentValue = dto.CurrentValue,
            PurchaseCost = dto.CurrentValue,
            PurchaseDate = DateTime.UtcNow,
            Description = dto.Description
        };
        await _context.Assets.AddAsync(asset);
        await _context.SaveChangesAsync();
        dto.Id = asset.Id;
        return ApiResponse<AssetDto>.Ok(dto);
    }

    public async Task<ApiResponse<List<JournalEntryDto>>> GetJournalEntriesAsync()
    {
        var list = await _context.JournalEntries.AsNoTracking()
            .Select(j => new JournalEntryDto
            {
                Id = j.Id,
                EntryNumber = j.EntryNumber,
                EntryDate = j.EntryDate,
                Description = j.Description,
                TotalAmount = j.TotalAmount
            }).ToListAsync();
        return ApiResponse<List<JournalEntryDto>>.Ok(list);
    }

    public async Task<ApiResponse<JournalEntryDto>> CreateJournalEntryAsync(JournalEntryDto dto)
    {
        var entry = new JournalEntry
        {
            EntryNumber = string.IsNullOrWhiteSpace(dto.EntryNumber) ? "JE-" + DateTime.Now.Ticks : dto.EntryNumber,
            EntryDate = dto.EntryDate == default ? DateTime.UtcNow : dto.EntryDate,
            Description = dto.Description,
            TotalAmount = dto.TotalAmount
        };
        await _context.JournalEntries.AddAsync(entry);
        await _context.SaveChangesAsync();
        dto.Id = entry.Id;
        return ApiResponse<JournalEntryDto>.Ok(dto);
    }

    public async Task<ApiResponse<object>> GetLedgerAsync()
    {
        var totalIncome = await _context.Incomes.SumAsync(i => (decimal?)i.Amount) ?? 0;
        var totalExpense = await _context.Expenses.SumAsync(e => (decimal?)e.Amount) ?? 0;
        var totalAssets = await _context.Assets.SumAsync(a => (decimal?)a.CurrentValue) ?? 0;
        var totalLiabilities = await _context.Liabilities.SumAsync(l => (decimal?)l.Amount - l.PaidAmount) ?? 0;
        
        var ledgerData = new
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            NetProfit = totalIncome - totalExpense,
            TotalAssets = totalAssets,
            TotalLiabilities = totalLiabilities,
            Equity = totalAssets - totalLiabilities,
            LastUpdated = DateTime.UtcNow
        };
        return ApiResponse<object>.Ok(ledgerData);
    }

    public async Task<ApiResponse<object>> GetSummaryReportAsync()
    {
        var todaySales = await _context.SalesInvoices
            .Where(s => s.SaleDate.Date == DateTime.UtcNow.Date)
            .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;
            
        var monthSales = await _context.SalesInvoices
            .Where(s => s.SaleDate.Month == DateTime.UtcNow.Month && s.SaleDate.Year == DateTime.UtcNow.Year)
            .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

        var reportData = new
        {
            ReportDate = DateTime.UtcNow,
            TodaySales = todaySales,
            CurrentMonthSales = monthSales,
            TotalCustomers = await _context.Customers.CountAsync(),
            TotalSuppliers = await _context.Suppliers.CountAsync(),
            TotalPartsInStock = await _context.WarehouseStocks.SumAsync(ws => (int?)ws.Quantity) ?? 0
        };
        return ApiResponse<object>.Ok(reportData);
    }
}
