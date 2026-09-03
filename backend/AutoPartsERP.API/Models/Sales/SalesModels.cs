namespace AutoPartsERP.API.Models.Sales;

using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;

public class SalesInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public string SaleType { get; set; } = "POS"; // POS, Wholesale, Online, Service
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string PaymentStatus { get; set; } = "Paid"; // Paid, Partial, Due
    public string Status { get; set; } = "Completed";   // Draft, Completed, Cancelled
    public string? Notes { get; set; }

    public int? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
    public ICollection<SalesReturn> Returns { get; set; } = new List<SalesReturn>();
}

public class SalesItem
{
    public int Id { get; set; }

    public int SalesInvoiceId { get; set; }
    public SalesInvoice SalesInvoice { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
}

public class SalesReturn
{
    public int Id { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;

    public int? SalesInvoiceId { get; set; }
    public SalesInvoice? SalesInvoice { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
    public decimal TotalRefundAmount { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "Completed";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SalesReturnItem> Items { get; set; } = new List<SalesReturnItem>();
}

public class SalesReturnItem
{
    public int Id { get; set; }

    public int SalesReturnId { get; set; }
    public SalesReturn SalesReturn { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class Quotation
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected, Converted
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<QuotationItem> Items { get; set; } = new List<QuotationItem>();
}

public class QuotationItem
{
    public int Id { get; set; }

    public int QuotationId { get; set; }
    public Quotation Quotation { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
