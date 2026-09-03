namespace AutoPartsERP.API.Models.Catalog;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Part> Parts { get; set; } = new List<Part>();
}

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Part> Parts { get; set; } = new List<Part>();
}

public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Part> Parts { get; set; } = new List<Part>();
}

public class Vehicle
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int YearStart { get; set; }
    public int? YearEnd { get; set; }
    public string? Engine { get; set; }
    public string? FuelType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PartVehicleCompatibility> CompatibleParts { get; set; } = new List<PartVehicleCompatibility>();
}

public class Part
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string? OEMNumber { get; set; }
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinStockAlert { get; set; } = 5;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigations
    public ICollection<PartVehicleCompatibility> CompatibleVehicles { get; set; } = new List<PartVehicleCompatibility>();
    public ICollection<Inventory.WarehouseStock> WarehouseStocks { get; set; } = new List<Inventory.WarehouseStock>();
}

public class PartVehicleCompatibility
{
    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string? Notes { get; set; }
}
