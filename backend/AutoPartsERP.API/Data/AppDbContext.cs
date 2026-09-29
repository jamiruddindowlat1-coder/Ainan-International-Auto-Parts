using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Models.Purchase;
using AutoPartsERP.API.Models.Sales;
using AutoPartsERP.API.Models.Accounting;
using AutoPartsERP.API.Models.Settings;

namespace AutoPartsERP.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 1. Auth & Users
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    // 2. Catalog
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<PartVehicleCompatibility> PartVehicleCompatibilities => Set<PartVehicleCompatibility>();

    // 3. Inventory & Warehouses
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    // 4. Partners
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Customer> Customers => Set<Customer>();

    // 5. Purchases
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
    public DbSet<PurchaseReturn> PurchaseReturns => Set<PurchaseReturn>();
    public DbSet<PurchaseReturnItem> PurchaseReturnItems => Set<PurchaseReturnItem>();

    // 6. Sales
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesItem> SalesItems => Set<SalesItem>();
    public DbSet<SalesReturn> SalesReturns => Set<SalesReturn>();
    public DbSet<SalesReturnItem> SalesReturnItems => Set<SalesReturnItem>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationItem> QuotationItems => Set<QuotationItem>();

    // 7. Accounting
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalItem> JournalItems => Set<JournalItem>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Liability> Liabilities => Set<Liability>();

    // 8. Settings
    public DbSet<CompanySetting> CompanySettings => Set<CompanySetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Decimal Precisions globally
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        // --- AUTH & ROLES ---
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        // --- CATALOG ---
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Brand>()
            .HasIndex(b => b.Name).IsUnique();
        modelBuilder.Entity<Unit>()
            .HasIndex(u => u.Name).IsUnique();

        modelBuilder.Entity<Part>()
            .HasIndex(p => p.PartNumber).IsUnique();
        modelBuilder.Entity<Part>()
            .HasIndex(p => p.OEMNumber);
        modelBuilder.Entity<Part>()
            .HasIndex(p => p.Barcode);

        modelBuilder.Entity<Part>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Parts)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Part>()
            .HasOne(p => p.Brand)
            .WithMany(b => b.Parts)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Part>()
            .HasOne(p => p.Unit)
            .WithMany(u => u.Parts)
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartVehicleCompatibility>()
            .HasKey(pvc => new { pvc.PartId, pvc.VehicleId });

        modelBuilder.Entity<PartVehicleCompatibility>()
            .HasOne(pvc => pvc.Part)
            .WithMany(p => p.CompatibleVehicles)
            .HasForeignKey(pvc => pvc.PartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PartVehicleCompatibility>()
            .HasOne(pvc => pvc.Vehicle)
            .WithMany(v => v.CompatibleParts)
            .HasForeignKey(pvc => pvc.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- INVENTORY ---
        modelBuilder.Entity<Warehouse>()
            .HasIndex(w => w.Code).IsUnique();

        modelBuilder.Entity<WarehouseStock>()
            .HasIndex(ws => new { ws.WarehouseId, ws.PartId }).IsUnique();

        modelBuilder.Entity<WarehouseStock>()
            .HasOne(ws => ws.Warehouse)
            .WithMany(w => w.WarehouseStocks)
            .HasForeignKey(ws => ws.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WarehouseStock>()
            .HasOne(ws => ws.Part)
            .WithMany(p => p.WarehouseStocks)
            .HasForeignKey(ws => ws.PartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StockMovement>()
            .HasOne(sm => sm.Part)
            .WithMany()
            .HasForeignKey(sm => sm.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockMovement>()
            .HasOne(sm => sm.Warehouse)
            .WithMany()
            .HasForeignKey(sm => sm.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- PURCHASES ---
        modelBuilder.Entity<PurchaseInvoice>()
            .HasIndex(pi => pi.InvoiceNumber).IsUnique();

        modelBuilder.Entity<PurchaseInvoice>()
            .HasOne(pi => pi.Supplier)
            .WithMany(s => s.PurchaseInvoices)
            .HasForeignKey(pi => pi.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseInvoice>()
            .HasOne(pi => pi.Warehouse)
            .WithMany()
            .HasForeignKey(pi => pi.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.PurchaseInvoice)
            .WithMany(p => p.Items)
            .HasForeignKey(pi => pi.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne(pi => pi.Part)
            .WithMany()
            .HasForeignKey(pi => pi.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseReturn>()
            .HasIndex(pr => pr.ReturnNumber).IsUnique();

        modelBuilder.Entity<PurchaseReturn>()
            .HasOne(pr => pr.Supplier)
            .WithMany(s => s.PurchaseReturns)
            .HasForeignKey(pr => pr.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseReturn>()
            .HasOne(pr => pr.Warehouse)
            .WithMany()
            .HasForeignKey(pr => pr.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseReturnItem>()
            .HasOne(pri => pri.PurchaseReturn)
            .WithMany(pr => pr.Items)
            .HasForeignKey(pri => pri.PurchaseReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- SALES ---
        modelBuilder.Entity<SalesInvoice>()
            .HasIndex(si => si.InvoiceNumber).IsUnique();

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Customer)
            .WithMany(c => c.SalesInvoices)
            .HasForeignKey(si => si.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesInvoice>()
            .HasOne(si => si.Warehouse)
            .WithMany()
            .HasForeignKey(si => si.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesItem>()
            .HasOne(si => si.SalesInvoice)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesItem>()
            .HasOne(si => si.Part)
            .WithMany()
            .HasForeignKey(si => si.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesReturn>()
            .HasIndex(sr => sr.ReturnNumber).IsUnique();

        modelBuilder.Entity<SalesReturn>()
            .HasOne(sr => sr.Customer)
            .WithMany(c => c.SalesReturns)
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesReturn>()
            .HasOne(sr => sr.Warehouse)
            .WithMany()
            .HasForeignKey(sr => sr.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesReturnItem>()
            .HasOne(sri => sri.SalesReturn)
            .WithMany(sr => sr.Items)
            .HasForeignKey(sri => sri.SalesReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Quotation>()
            .HasIndex(q => q.QuotationNumber).IsUnique();

        modelBuilder.Entity<Quotation>()
            .HasOne(q => q.Customer)
            .WithMany(c => c.Quotations)
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QuotationItem>()
            .HasOne(qi => qi.Quotation)
            .WithMany(q => q.Items)
            .HasForeignKey(qi => qi.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- ACCOUNTING ---
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountCode).IsUnique();

        modelBuilder.Entity<Account>()
            .HasOne(a => a.ParentAccount)
            .WithMany(p => p.ChildAccounts)
            .HasForeignKey(a => a.ParentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JournalEntry>()
            .HasIndex(je => je.EntryNumber).IsUnique();

        modelBuilder.Entity<JournalItem>()
            .HasOne(ji => ji.JournalEntry)
            .WithMany(je => je.Items)
            .HasForeignKey(ji => ji.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JournalItem>()
            .HasOne(ji => ji.Account)
            .WithMany(a => a.JournalItems)
            .HasForeignKey(ji => ji.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Expense>()
            .HasIndex(e => e.ExpenseNumber).IsUnique();

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.ExpenseCategory)
            .WithMany(ec => ec.Expenses)
            .HasForeignKey(e => e.ExpenseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Account)
            .WithMany(a => a.Expenses)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Income>()
            .HasIndex(i => i.IncomeNumber).IsUnique();

        modelBuilder.Entity<Income>()
            .HasOne(i => i.Account)
            .WithMany(a => a.Incomes)
            .HasForeignKey(i => i.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.AssetCode).IsUnique();
    }
}
