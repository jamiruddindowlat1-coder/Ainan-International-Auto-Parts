namespace AutoPartsERP.API.DTOs;

public class PostJournalEntryDto
{
    public string? Description { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? EntryDate { get; set; }
    public List<JournalLineDto> Items { get; set; } = new();
}

public class JournalLineDto
{
    public int AccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}

public class JournalPostResultDto
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class RecordExpenseDto
{
    public int ExpenseCategoryId { get; set; }
    public int? AccountId { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Reference { get; set; }
    public string? Note { get; set; }
}

public class ExpenseResultDto
{
    public int Id { get; set; }
    public string ExpenseNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
}