namespace AutoPartsERP.API.Models.Inventory;

using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Models.Catalog;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
}

public class WarehouseStock
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int Quantity { get; set; }
    public string? RackLocation { get; set; }
    public string? BinLocation { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class StockMovement
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public string MovementType { get; set; } = string.Empty; // 'Purchase', 'Sale', 'PurchaseReturn', 'SalesReturn', 'Adjustment', 'Transfer'
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
