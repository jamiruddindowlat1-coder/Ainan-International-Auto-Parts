namespace AutoPartsERP.API.Models.Accounting;

using AutoPartsERP.API.Models.Auth;

public class Account
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = "Asset"; // Asset, Liability, Equity, Revenue, Expense
    public int? ParentAccountId { get; set; }
    public Account? ParentAccount { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
    public ICollection<JournalItem> JournalItems { get; set; } = new List<JournalItem>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
}

public class JournalEntry
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; } = DateTime.UtcNow;
    public string? ReferenceNumber { get; set; }
    public string? Description { get; set; }
    public decimal TotalAmount { get; set; }

    public int? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<JournalItem> Items { get; set; } = new List<JournalItem>();
}

public class JournalItem
{
    public int Id { get; set; }

    public int JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}

public class ExpenseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}

public class Expense
{
    public int Id { get; set; }
    public string ExpenseNumber { get; set; } = string.Empty;

    public int ExpenseCategoryId { get; set; }
    public ExpenseCategory ExpenseCategory { get; set; } = null!;

    public int? AccountId { get; set; }
    public Account? Account { get; set; }

    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
    public string PaymentMethod { get; set; } = "Cash";
    public string? Reference { get; set; }
    public string? Note { get; set; }

    public int? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Income
{
    public int Id { get; set; }
    public string IncomeNumber { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;

    public int? AccountId { get; set; }
    public Account? Account { get; set; }

    public decimal Amount { get; set; }
    public DateTime IncomeDate { get; set; } = DateTime.UtcNow;
    public string PaymentMethod { get; set; } = "Cash";
    public string? Reference { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Asset
{
    public int Id { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string AssetCode { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal PurchaseCost { get; set; }
    public decimal DepreciationRate { get; set; }
    public decimal CurrentValue { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Liability
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CreditorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal PaidAmount { get; set; }
    public string Status { get; set; } = "Active";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
