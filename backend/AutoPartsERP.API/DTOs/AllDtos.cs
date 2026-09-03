namespace AutoPartsERP.API.DTOs;

// --- AUTH DTOs ---
public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
}

public class RegisterUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public List<int> RoleIds { get; set; } = new();
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

// --- CATALOG DTOs ---
public class PartDto
{
    public int Id { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string? OEMNumber { get; set; }
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int BrandId { get; set; }
    public string? BrandName { get; set; }
    public int UnitId { get; set; }
    public string? UnitName { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinStockAlert { get; set; }
    public int TotalStock { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}

public class CreatePartDto
{
    public string PartNumber { get; set; } = string.Empty;
    public string? OEMNumber { get; set; }
    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public int UnitId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinStockAlert { get; set; } = 5;
    public string? ImageUrl { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int PartsCount { get; set; }
    public bool IsActive { get; set; }
}

public class BrandDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public int PartsCount { get; set; }
    public bool IsActive { get; set; }
}

public class UnitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class VehicleDto
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int YearStart { get; set; }
    public int? YearEnd { get; set; }
    public string? Engine { get; set; }
    public string? FuelType { get; set; }
}

// --- PARTNERS DTOs ---
public class SupplierDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; }
}

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CustomerType { get; set; } = "Retail";
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; }
}

// --- SALES & POS DTOs ---
public class CreateSalesInvoiceDto
{
    public int CustomerId { get; set; }
    public int WarehouseId { get; set; }
    public string SaleType { get; set; } = "POS";
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string? Notes { get; set; }
    public List<SalesItemInputDto> Items { get; set; } = new();
}

public class SalesItemInputDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
}

public class SalesInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string SaleType { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<SalesItemDto> Items { get; set; } = new();
}

public class SalesItemDto
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
}

// --- PURCHASES DTOs ---
public class CreatePurchaseInvoiceDto
{
    public int SupplierId { get; set; }
    public int WarehouseId { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; } = "Bank";
    public string? Notes { get; set; }
    public List<PurchaseItemInputDto> Items { get; set; } = new();
}

public class PurchaseItemInputDto
{
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class PurchaseInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<PurchaseItemDto> Items { get; set; } = new();
}

public class PurchaseItemDto
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
}

// --- DASHBOARD DTOs ---
public class DashboardSummaryDto
{
    public decimal TodaySales { get; set; }
    public decimal MonthSales { get; set; }
    public decimal TotalReceivable { get; set; }
    public decimal TotalPayable { get; set; }
    public int TotalParts { get; set; }
    public int LowStockCount { get; set; }
    public int TodayInvoicesCount { get; set; }
    public decimal MonthlyExpense { get; set; }
    public List<MonthlySalesTrendDto> MonthlySalesTrend { get; set; } = new();
    public List<TopSellingPartDto> TopSellingParts { get; set; } = new();
    public List<LowStockPartDto> LowStockAlerts { get; set; } = new();
    public List<RecentSaleDto> RecentSales { get; set; } = new();
}

public class MonthlySalesTrendDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Sales { get; set; }
    public decimal Purchases { get; set; }
    public decimal Expenses { get; set; }
}

public class TopSellingPartDto
{
    public string PartNumber { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class LowStockPartDto
{
    public int PartId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string PartName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinStockAlert { get; set; }
}

public class RecentSaleDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
