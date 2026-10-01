namespace AutoPartsERP.API.DTOs;

public class CreatePurchaseReturnDto
{
    public int PurchaseInvoiceId { get; set; }
    public int WarehouseId { get; set; }
    public string? Reason { get; set; }
    public List<PurchaseReturnItemInputDto> Items { get; set; } = new();
}

public class PurchaseReturnItemInputDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
}

public class PurchaseReturnResultDto
{
    public int Id { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AppliedToBalance { get; set; }
    public decimal CashRefund { get; set; }
}