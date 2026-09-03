namespace AutoPartsERP.API.Models.Purchase;

using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;

public class PurchaseInvoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentStatus { get; set; } = "Pending"; // Paid, Partial, Due
    public string? PaymentMethod { get; set; }
    public string Status { get; set; } = "Received"; // Pending, Received, Cancelled
    public string? Notes { get; set; }

    public int? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    public ICollection<PurchaseReturn> Returns { get; set; } = new List<PurchaseReturn>();
}

public class PurchaseItem
{
    public int Id { get; set; }

    public int PurchaseInvoiceId { get; set; }
    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
}

public class PurchaseReturn
{
    public int Id { get; set; }
    public string ReturnNumber { get; set; } = string.Empty;

    public int? PurchaseInvoiceId { get; set; }
    public PurchaseInvoice? PurchaseInvoice { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
    public decimal TotalRefundAmount { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "Completed";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PurchaseReturnItem> Items { get; set; } = new List<PurchaseReturnItem>();
}

public class PurchaseReturnItem
{
    public int Id { get; set; }

    public int PurchaseReturnId { get; set; }
    public PurchaseReturn PurchaseReturn { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}
