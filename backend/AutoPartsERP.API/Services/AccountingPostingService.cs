using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Accounting;

namespace AutoPartsERP.API.Services;

public class AccountingPostingService : IAccountingPostingService
{
    private readonly AppDbContext _context;

    public AccountingPostingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<JournalPostResultDto>> PostJournalEntryAsync(PostJournalEntryDto dto, int? userId)
    {
        if (dto.Items == null || dto.Items.Count < 2)
            return ApiResponse<JournalPostResultDto>.Fail("A journal entry requires at least two lines (Debit and Credit).");

        foreach (var line in dto.Items)
        {
            if (line.Debit < 0 || line.Credit < 0)
                return ApiResponse<JournalPostResultDto>.Fail("Debit and Credit amounts cannot be negative.");

            if ((line.Debit > 0) == (line.Credit > 0))
                return ApiResponse<JournalPostResultDto>.Fail("Each line must have either a Debit or a Credit amount, not both and not neither.");
        }

        var totalDebit = Math.Round(dto.Items.Sum(i => i.Debit), 2);
        var totalCredit = Math.Round(dto.Items.Sum(i => i.Credit), 2);

        if (totalDebit != totalCredit)
            return ApiResponse<JournalPostResultDto>.Fail(
                $"Journal is out of balance! Total Debit ({totalDebit}) must equal Total Credit ({totalCredit}).");

        var ids = dto.Items.Select(i => i.AccountId).Distinct().ToList();
        var accounts = await _context.Accounts.Where(a => ids.Contains(a.Id)).ToListAsync();

        foreach (var id in ids)
        {
            var account = accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
                return ApiResponse<JournalPostResultDto>.Fail($"Account ID {id} not found");
            if (!account.IsActive)
                return ApiResponse<JournalPostResultDto>.Fail($"Account ID {id} is inactive");
        }

        var entryNo = "JV-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

        var entry = new JournalEntry
        {
            EntryNumber = entryNo,
            EntryDate = dto.EntryDate ?? DateTime.UtcNow,
            ReferenceNumber = dto.ReferenceNumber,
            Description = dto.Description,
            TotalAmount = totalDebit,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var line in dto.Items)
        {
            var account = accounts.First(a => a.Id == line.AccountId);

            entry.Items.Add(new JournalItem
            {
                AccountId = line.AccountId,
                Debit = line.Debit,
                Credit = line.Credit,
                Description = line.Description ?? dto.Description
            });

            // Debit-normal: Asset/Expense. Credit-normal: Liability/Equity/Revenue.
            if (account.AccountType == "Asset" || account.AccountType == "Expense")
                account.Balance += line.Debit - line.Credit;
            else
                account.Balance += line.Credit - line.Debit;
        }

        await _context.JournalEntries.AddAsync(entry);
        await _context.SaveChangesAsync();

        return ApiResponse<JournalPostResultDto>.Ok(new JournalPostResultDto
        {
            Id = entry.Id,
            EntryNumber = entry.EntryNumber,
            TotalAmount = entry.TotalAmount
        }, "Journal Entry posted successfully");
    }

    public async Task<ApiResponse<ExpenseResultDto>> RecordExpenseAsync(RecordExpenseDto dto, int? userId)
    {
        if (dto.Amount <= 0)
            return ApiResponse<ExpenseResultDto>.Fail("Amount must be greater than zero");

        if (!await _context.ExpenseCategories.AnyAsync(c => c.Id == dto.ExpenseCategoryId))
            return ApiResponse<ExpenseResultDto>.Fail($"Expense category ID {dto.ExpenseCategoryId} not found");

        if (dto.AccountId.HasValue)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == dto.AccountId.Value);
            if (account == null)
                return ApiResponse<ExpenseResultDto>.Fail($"Account ID {dto.AccountId.Value} not found");
            if (!account.IsActive)
                return ApiResponse<ExpenseResultDto>.Fail($"Account ID {dto.AccountId.Value} is inactive");
        }

        var expense = new Expense
        {
            ExpenseNumber = "EXP-" + DateTime.UtcNow.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
            ExpenseCategoryId = dto.ExpenseCategoryId,
            AccountId = dto.AccountId,
            Amount = Math.Round(dto.Amount, 2),
            ExpenseDate = DateTime.UtcNow,
            PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? "Cash" : dto.PaymentMethod.Trim(),
            Reference = dto.Reference,
            Note = dto.Note,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        return ApiResponse<ExpenseResultDto>.Ok(new ExpenseResultDto
        {
            Id = expense.Id,
            ExpenseNumber = expense.ExpenseNumber,
            Amount = expense.Amount,
            ExpenseDate = expense.ExpenseDate
        }, "Expense recorded successfully");
    }
}