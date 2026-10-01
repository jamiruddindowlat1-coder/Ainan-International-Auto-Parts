namespace AutoPartsERP.API.DTOs;

public class CreateQuotationDto
{
    public int CustomerId { get; set; }
    public int ValidDays { get; set; } = 30;
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public string? Notes { get; set; }
    public List<QuotationItemInputDto> Items { get; set; } = new();
}

public class QuotationItemInputDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}

public class QuotationResultDto
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? ExpiryDate { get; set; }
}