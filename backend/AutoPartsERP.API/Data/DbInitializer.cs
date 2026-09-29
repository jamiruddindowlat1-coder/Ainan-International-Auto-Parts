using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Models.Catalog;
using AutoPartsERP.API.Models.Inventory;
using AutoPartsERP.API.Models.Partners;
using AutoPartsERP.API.Models.Accounting;
using AutoPartsERP.API.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.API.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "SuperAdmin", Description = "Complete system administrator with full access" },
                new() { Name = "Admin", Description = "Operations manager with operational access" },
                new() { Name = "SalesStaff", Description = "Cashier and sales counter personnel" },
                new() { Name = "InventoryManager", Description = "Warehouse and inventory control personnel" },
                new() { Name = "Accountant", Description = "Financial, ledger and expense management personnel" }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // 2. Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@aiaps.com",
                // Hashed password for 'Admin@123'
                PasswordHash = "AQAAAAEAACcQAAAAEJ8+3f6n4/KzQk/r6Q0tYgN8V5z7u0Z5gqV+Q9l4H8J1zV5m7Y1eT4W6g==",
                FullName = "System Administrator",
                Phone = "+880 1700-000001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            var superAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
            if (superAdminRole != null)
            {
                await context.UserRoles.AddAsync(new UserRole { UserId = adminUser.Id, RoleId = superAdminRole.Id });
                await context.SaveChangesAsync();
            }
        }

        // 3. Seed Units
        if (!await context.Units.AnyAsync())
        {
            var units = new List<Unit>
            {
                new() { Name = "Pieces", ShortCode = "Pcs", IsActive = true },
                new() { Name = "Set", ShortCode = "Set", IsActive = true },
                new() { Name = "Box", ShortCode = "Box", IsActive = true },
                new() { Name = "Pair", ShortCode = "Pair", IsActive = true },
                new() { Name = "Liter", ShortCode = "Ltr", IsActive = true },
                new() { Name = "Meter", ShortCode = "Mtr", IsActive = true },
                new() { Name = "Kilogram", ShortCode = "Kg", IsActive = true }
            };
            await context.Units.AddRangeAsync(units);
            await context.SaveChangesAsync();
        }

        // 4. Seed Categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Engine Parts", Code = "ENG", Description = "Pistons, Valves, Gaskets, Timing Belts, Crankshafts" },
                new() { Name = "Brake System", Code = "BRK", Description = "Brake Pads, Brake Shoes, Rotors, Calipers, Master Cylinders" },
                new() { Name = "Suspension & Steering", Code = "SUS", Description = "Shock Absorbers, Control Arms, Ball Joints, Tie Rods" },
                new() { Name = "Electrical & Lighting", Code = "ELE", Description = "Headlights, Alternators, Starters, Spark Plugs, Batteries" },
                new() { Name = "Filtration", Code = "FLT", Description = "Oil Filters, Air Filters, Fuel Filters, Cabin Filters" },
                new() { Name = "Transmission & Clutch", Code = "TRN", Description = "Clutch Plates, Pressure Plates, Gearbox Components" },
                new() { Name = "Cooling & Heating", Code = "COL", Description = "Radiators, Water Pumps, Thermostats, Coolant" },
                new() { Name = "Body & Exterior", Code = "BDY", Description = "Bumpers, Mirrors, Fenders, Grilles, Door Handles" },
                new() { Name = "Lubricants & Fluids", Code = "LUB", Description = "Engine Oils, Transmission Fluid, Brake Fluid" }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // 5. Seed Brands
        if (!await context.Brands.AnyAsync())
        {
            var brands = new List<Brand>
            {
                new() { Name = "Bosch", Country = "Germany", Description = "High quality electrical, braking and ignition components" },
                new() { Name = "Denso", Country = "Japan", Description = "Spark plugs, alternators, radiators, sensors" },
                new() { Name = "NGK", Country = "Japan", Description = "World-leading spark plugs and glow plugs" },
                new() { Name = "Toyota Genuine Parts", Country = "Japan", Description = "Original equipment manufacturer for Toyota" },
                new() { Name = "Brembo", Country = "Italy", Description = "High performance braking systems and pads" },
                new() { Name = "KYB", Country = "Japan", Description = "Premium shock absorbers and struts" },
                new() { Name = "Mann-Filter", Country = "Germany", Description = "Premium filtration products" },
                new() { Name = "Mobil 1", Country = "USA", Description = "Synthetic motor oils and lubricants" },
                new() { Name = "AISIN", Country = "Japan", Description = "Clutch kits, water pumps, transmission parts" }
            };
            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        // 6. Seed Warehouses
        if (!await context.Warehouses.AnyAsync())
        {
            var warehouses = new List<Warehouse>
            {
                new() { Name = "Main Central Warehouse", Code = "WH-MAIN", Location = "Dhaka Hub - Tejgaon Industrial Area", ContactPerson = "Tareq Ahmed", Phone = "+880 1711-000001", IsDefault = true, IsActive = true },
                new() { Name = "Chittagong Port Warehouse", Code = "WH-CTG", Location = "Agrabad Commercial Area, Chittagong", ContactPerson = "Kamal Hossain", Phone = "+880 1711-000002", IsDefault = false, IsActive = true },
                new() { Name = "Uttara Showroom & Store", Code = "WH-UTT", Location = "Sector 3, Uttara, Dhaka", ContactPerson = "Farhan Kabir", Phone = "+880 1711-000003", IsDefault = false, IsActive = true }
            };
            await context.Warehouses.AddRangeAsync(warehouses);
            await context.SaveChangesAsync();
        }

        // 7. Seed Chart of Accounts
        if (!await context.Accounts.AnyAsync())
        {
            var accounts = new List<Account>
            {
                new() { AccountCode = "1010", AccountName = "Cash on Hand", AccountType = "Asset", Balance = 150000.00m },
                new() { AccountCode = "1020", AccountName = "City Bank - Corporate Account", AccountType = "Asset", Balance = 850000.00m },
                new() { AccountCode = "1030", AccountName = "Accounts Receivable (Customers)", AccountType = "Asset", Balance = 109500.00m },
                new() { AccountCode = "1040", AccountName = "Merchandise Inventory", AccountType = "Asset", Balance = 780000.00m },
                new() { AccountCode = "2010", AccountName = "Accounts Payable (Suppliers)", AccountType = "Liability", Balance = 515000.00m },
                new() { AccountCode = "2020", AccountName = "Short Term Loan / Credit", AccountType = "Liability", Balance = 200000.00m },
                new() { AccountCode = "3010", AccountName = "Owner Capital / Equity", AccountType = "Equity", Balance = 1000000.00m },
                new() { AccountCode = "4010", AccountName = "Auto Parts Sales Revenue", AccountType = "Revenue", Balance = 0.00m },
                new() { AccountCode = "4020", AccountName = "Service & Labor Income", AccountType = "Revenue", Balance = 0.00m },
                new() { AccountCode = "5010", AccountName = "Cost of Goods Sold (COGS)", AccountType = "Expense", Balance = 0.00m },
                new() { AccountCode = "5020", AccountName = "Showroom & Warehouse Rent", AccountType = "Expense", Balance = 0.00m },
                new() { AccountCode = "5030", AccountName = "Staff Salaries & Benefits", AccountType = "Expense", Balance = 0.00m },
                new() { AccountCode = "5040", AccountName = "Electricity & Utilities", AccountType = "Expense", Balance = 0.00m }
            };
            await context.Accounts.AddRangeAsync(accounts);
            await context.SaveChangesAsync();
        }

        // 8. Seed Expense Categories
        if (!await context.ExpenseCategories.AnyAsync())
        {
            var expCats = new List<ExpenseCategory>
            {
                new() { Name = "Rent & Facilities", Description = "Office, warehouse and showroom rent" },
                new() { Name = "Salaries & Payroll", Description = "Staff monthly wages and incentives" },
                new() { Name = "Utilities & Bills", Description = "Electricity, Internet, Water and Generator Fuel" },
                new() { Name = "Transport & Shipping", Description = "Freight intake, port handling and local delivery" },
                new() { Name = "Maintenance & Office", Description = "Store maintenance, tea, printing and stationery" }
            };
            await context.ExpenseCategories.AddRangeAsync(expCats);
            await context.SaveChangesAsync();
        }

        // 9. Seed Company Settings
        if (!await context.CompanySettings.AnyAsync())
        {
            var setting = new CompanySetting
            {
                CompanyName = "Ainan International Auto Parts System",
                ShortName = "AIAPS",
                Tagline = "Your Trusted Auto Parts Partner Worldwide",
                Email = "info@aiaps.com",
                Phone = "+880 1700-000000",
                Address = "Tejgaon Industrial Area, Dhaka, Bangladesh",
                Website = "www.aiaps.com",
                CurrencyCode = "BDT",
                CurrencySymbol = "৳",
                FooterText = "Thank you for your business | AIAPS"
            };
            await context.CompanySettings.AddAsync(setting);
            await context.SaveChangesAsync();
        }

        // 10. Seed Sample Customers & Suppliers & Parts
        if (!await context.Customers.AnyAsync())
        {
            var customers = new List<Customer>
            {
                new() { Name = "Apex Auto Garage", CustomerType = "Garage", Phone = "+880 1812-345678", Email = "apex@autogarage.com", Address = "Mirpur-10, Dhaka", CreditLimit = 150000.00m, CurrentBalance = 24500.00m },
                new() { Name = "Rahim Motors Wholesale", CustomerType = "Wholesale", Phone = "+880 1913-987654", Email = "rahim@motorsbd.com", Address = "Dholaikhal, Old Dhaka", CreditLimit = 500000.00m, CurrentBalance = 85000.00m },
                new() { Name = "Kabir Express Fleet", CustomerType = "Corporate", Phone = "+880 1715-112233", Email = "fleet@kabirexpress.com", Address = "Mohakhali, Dhaka", CreditLimit = 300000.00m, CurrentBalance = 0.00m },
                new() { Name = "Walk-in Customer (Counter)", CustomerType = "Retail", Phone = "+880 1700-000000", Email = "retail@aiaps.com", Address = "Store Counter", CreditLimit = 0.00m, CurrentBalance = 0.00m }
            };
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        if (!await context.Suppliers.AnyAsync())
        {
            var suppliers = new List<Supplier>
            {
                new() { Name = "Global Auto Parts Trading Dubai", Company = "Global Parts FZE", Phone = "+971 4 1234567", Email = "sales@globalpartsfze.ae", Address = "Al Quoz Industrial, Dubai, UAE", CurrentBalance = 120000.00m },
                new() { Name = "Nippon Auto Exports Tokyo", Company = "Nippon Exports Co.", Phone = "+81 3 55556666", Email = "orders@nipponexports.jp", Address = "Yokohama, Japan", CurrentBalance = 350000.00m },
                new() { Name = "Bangla Motor Spares Importers", Company = "BMS Importers Ltd.", Phone = "+880 2 9887766", Email = "info@bmsimporters.com", Address = "Motijheel, Dhaka", CurrentBalance = 45000.00m }
            };
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }

        if (!await context.Parts.AnyAsync())
        {
            var catBrake = await context.Categories.FirstAsync(c => c.Name == "Brake System");
            var catElec = await context.Categories.FirstAsync(c => c.Name == "Electrical & Lighting");
            var catFilt = await context.Categories.FirstAsync(c => c.Name == "Filtration");
            var catSusp = await context.Categories.FirstAsync(c => c.Name == "Suspension & Steering");
            var catLube = await context.Categories.FirstAsync(c => c.Name == "Lubricants & Fluids");

            var brandBosch = await context.Brands.FirstAsync(b => b.Name == "Bosch");
            var brandDenso = await context.Brands.FirstAsync(b => b.Name == "Denso");
            var brandMann = await context.Brands.FirstAsync(b => b.Name == "Mann-Filter");
            var brandKyb = await context.Brands.FirstAsync(b => b.Name == "KYB");
            var brandMobil = await context.Brands.FirstAsync(b => b.Name == "Mobil 1");

            var unitPcs = await context.Units.FirstAsync(u => u.ShortCode == "Pcs");
            var unitPair = await context.Units.FirstAsync(u => u.ShortCode == "Pair");

            var parts = new List<Part>
            {
                new() { PartNumber = "BOS-BP-001", OEMNumber = "04465-02220", Barcode = "890123450001", Name = "Front Ceramic Brake Pad Set", Description = "Low dust high performance ceramic brake pads", CategoryId = catBrake.Id, BrandId = brandBosch.Id, UnitId = unitPair.Id, CostPrice = 2200.00m, SellingPrice = 3200.00m, WholesalePrice = 2700.00m, MinStockAlert = 10 },
                new() { PartNumber = "DEN-SP-IX01", OEMNumber = "90919-01210", Barcode = "890123450002", Name = "Iridium Power Spark Plug (SK20R11)", Description = "High ignition efficiency laser iridium spark plug", CategoryId = catElec.Id, BrandId = brandDenso.Id, UnitId = unitPcs.Id, CostPrice = 650.00m, SellingPrice = 950.00m, WholesalePrice = 800.00m, MinStockAlert = 30 },
                new() { PartNumber = "MNN-OF-W68", OEMNumber = "90915-YZZE1", Barcode = "890123450004", Name = "Mann Spin-On Engine Oil Filter", Description = "Micro-fiber high dirt holding oil filter", CategoryId = catFilt.Id, BrandId = brandMann.Id, UnitId = unitPcs.Id, CostPrice = 400.00m, SellingPrice = 650.00m, WholesalePrice = 520.00m, MinStockAlert = 20 },
                new() { PartNumber = "KYB-SA-33331", OEMNumber = "48510-80255", Barcode = "890123450005", Name = "KYB Excel-G Front Strut Assembly", Description = "Twin tube gas strut for smooth comfort", CategoryId = catSusp.Id, BrandId = brandKyb.Id, UnitId = unitPcs.Id, CostPrice = 6500.00m, SellingPrice = 9200.00m, WholesalePrice = 7800.00m, MinStockAlert = 4 },
                new() { PartNumber = "MOB-OIL-5W30", OEMNumber = "MOBIL-SYN-4L", Barcode = "890123450006", Name = "Mobil 1 Full Synthetic Engine Oil (4L)", Description = "Advanced full synthetic formula", CategoryId = catLube.Id, BrandId = brandMobil.Id, UnitId = unitPcs.Id, CostPrice = 3800.00m, SellingPrice = 5200.00m, WholesalePrice = 4400.00m, MinStockAlert = 15 }
            };

            await context.Parts.AddRangeAsync(parts);
            await context.SaveChangesAsync();

            var mainWh = await context.Warehouses.FirstAsync(w => w.Code == "WH-MAIN");
            var stocks = new List<WarehouseStock>
            {
                new() { WarehouseId = mainWh.Id, PartId = parts[0].Id, Quantity = 45, RackLocation = "Rack-A1", BinLocation = "Bin-04" },
                new() { WarehouseId = mainWh.Id, PartId = parts[1].Id, Quantity = 120, RackLocation = "Rack-B2", BinLocation = "Bin-12" },
                new() { WarehouseId = mainWh.Id, PartId = parts[2].Id, Quantity = 95, RackLocation = "Rack-C1", BinLocation = "Bin-02" },
                new() { WarehouseId = mainWh.Id, PartId = parts[3].Id, Quantity = 12, RackLocation = "Rack-D3", BinLocation = "Bin-01" },
                new() { WarehouseId = mainWh.Id, PartId = parts[4].Id, Quantity = 38, RackLocation = "Rack-E1", BinLocation = "Bin-08" }
            };
            await context.WarehouseStocks.AddRangeAsync(stocks);
            await context.SaveChangesAsync();
        }
    }
}
