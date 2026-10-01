namespace AutoPartsERP.API.DTOs;

public class CreateSalesReturnDto
{
    public int SalesInvoiceId { get; set; }
    public int WarehouseId { get; set; }
    public string? Reason { get; set; }
    public List<SalesReturnItemInputDto> Items { get; set; } = new();
}

public class SalesReturnItemInputDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
}

public class SalesReturnResultDto
{
    public int Id { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AppliedToBalance { get; set; }
    public decimal CashRefund { get; set; }
}