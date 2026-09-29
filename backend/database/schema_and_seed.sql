-- ============================================================================
-- AIAPS - Ainan International Auto Parts System (AutoPartsERP)
-- Complete Database Schema with All Relationships, Constraints & Seed Data
-- ============================================================================

CREATE DATABASE AutoPartsERP_DB;
GO

USE AutoPartsERP_DB;
GO

-- 1. ROLES & PERMISSIONS
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Permissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ModuleName NVARCHAR(50) NOT NULL,
    ActionName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL,
    CONSTRAINT UQ_Permission_Module_Action UNIQUE (ModuleName, ActionName)
);

CREATE TABLE RolePermissions (
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    PRIMARY KEY (RoleId, PermissionId),
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE
);

-- 2. USERS
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2 NULL
);

CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- 3. MASTER DATA: CATEGORIES, BRANDS, UNITS, VEHICLES
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Code NVARCHAR(30) NULL,
    Description NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Brands (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Country NVARCHAR(100) NULL,
    Description NVARCHAR(255) NULL,
    LogoUrl NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Units (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    ShortCode NVARCHAR(10) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Vehicles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Make NVARCHAR(50) NOT NULL,            -- e.g. Toyota, Honda, Nissan
    Model NVARCHAR(50) NOT NULL,           -- e.g. Corolla, Civic, X-Trail
    YearStart INT NOT NULL,
    YearEnd INT NULL,
    Engine NVARCHAR(50) NULL,              -- e.g. 1.8L 2ZR-FE, 1.5L Turbo
    FuelType NVARCHAR(30) NULL,            -- Petrol, Diesel, Hybrid, Electric
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_Vehicle UNIQUE (Make, Model, YearStart, YearEnd, Engine)
);

-- 4. PARTS / PRODUCTS CATALOG
CREATE TABLE Parts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PartNumber NVARCHAR(50) NOT NULL UNIQUE,
    OEMNumber NVARCHAR(50) NULL,
    Barcode NVARCHAR(50) NULL,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    CategoryId INT NOT NULL,
    BrandId INT NOT NULL,
    UnitId INT NOT NULL,
    CostPrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    SellingPrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    WholesalePrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    MinStockAlert INT NOT NULL DEFAULT 5,
    ImageUrl NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Parts_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Parts_Brands FOREIGN KEY (BrandId) REFERENCES Brands(Id),
    CONSTRAINT FK_Parts_Units FOREIGN KEY (UnitId) REFERENCES Units(Id)
);
CREATE INDEX IX_Parts_PartNumber ON Parts(PartNumber);
CREATE INDEX IX_Parts_OEMNumber ON Parts(OEMNumber);
CREATE INDEX IX_Parts_Barcode ON Parts(Barcode);

-- Part Compatibility with Vehicles (Many-to-Many)
CREATE TABLE PartVehicleCompatibilities (
    PartId INT NOT NULL,
    VehicleId INT NOT NULL,
    Notes NVARCHAR(255) NULL,
    PRIMARY KEY (PartId, VehicleId),
    CONSTRAINT FK_PVC_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PVC_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id) ON DELETE CASCADE
);

-- 5. WAREHOUSES & INVENTORY
CREATE TABLE Warehouses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Code NVARCHAR(20) NOT NULL UNIQUE,
    Location NVARCHAR(200) NULL,
    ContactPerson NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    IsDefault BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE WarehouseStocks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WarehouseId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    RackLocation NVARCHAR(50) NULL,
    BinLocation NVARCHAR(50) NULL,
    LastUpdated DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_Warehouse_Part UNIQUE (WarehouseId, PartId),
    CONSTRAINT FK_WarehouseStock_Warehouse FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id) ON DELETE CASCADE,
    CONSTRAINT FK_WarehouseStock_Part FOREIGN KEY (PartId) REFERENCES Parts(Id) ON DELETE CASCADE
);

CREATE TABLE StockMovements (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PartId INT NOT NULL,
    WarehouseId INT NOT NULL,
    MovementType NVARCHAR(30) NOT NULL, -- 'Purchase', 'Sale', 'PurchaseReturn', 'SalesReturn', 'Adjustment', 'Transfer'
    Quantity INT NOT NULL,              -- Positive for IN, Negative for OUT
    UnitCost DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    ReferenceNumber NVARCHAR(50) NULL,  -- Invoice/Order number
    Notes NVARCHAR(255) NULL,
    UserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_StockMovements_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id),
    CONSTRAINT FK_StockMovements_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id),
    CONSTRAINT FK_StockMovements_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- 6. SUPPLIERS & CUSTOMERS
CREATE TABLE Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Company NVARCHAR(150) NULL,
    Phone NVARCHAR(30) NOT NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL,
    TaxNumber NVARCHAR(50) NULL,
    OpeningBalance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    CurrentBalance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    CustomerType NVARCHAR(30) NOT NULL DEFAULT 'Retail', -- 'Retail', 'Wholesale', 'Garage', 'Corporate'
    Phone NVARCHAR(30) NOT NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(255) NULL,
    CreditLimit DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    OpeningBalance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    CurrentBalance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 7. PURCHASES & PURCHASE RETURNS
CREATE TABLE PurchaseInvoices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
    SupplierId INT NOT NULL,
    WarehouseId INT NOT NULL,
    PurchaseDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SubTotal DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    ShippingCost DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DueAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    PaymentStatus NVARCHAR(30) NOT NULL DEFAULT 'Pending', -- 'Paid', 'Partial', 'Due'
    PaymentMethod NVARCHAR(50) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Received',       -- 'Pending', 'Received', 'Cancelled'
    Notes NVARCHAR(500) NULL,
    CreatedByUserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Purchase_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
    CONSTRAINT FK_Purchase_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id),
    CONSTRAINT FK_Purchase_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE TABLE PurchaseItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseInvoiceId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalCost DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_PurchaseItems_Invoice FOREIGN KEY (PurchaseInvoiceId) REFERENCES PurchaseInvoices(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PurchaseItems_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id)
);

CREATE TABLE PurchaseReturns (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReturnNumber NVARCHAR(50) NOT NULL UNIQUE,
    PurchaseInvoiceId INT NULL,
    SupplierId INT NOT NULL,
    WarehouseId INT NOT NULL,
    ReturnDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    TotalRefundAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Reason NVARCHAR(255) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Completed',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_PR_PurchaseInvoice FOREIGN KEY (PurchaseInvoiceId) REFERENCES PurchaseInvoices(Id),
    CONSTRAINT FK_PR_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
    CONSTRAINT FK_PR_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id)
);

CREATE TABLE PurchaseReturnItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PurchaseReturnId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitCost DECIMAL(18,2) NOT NULL,
    TotalCost DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_PRI_PurchaseReturn FOREIGN KEY (PurchaseReturnId) REFERENCES PurchaseReturns(Id) ON DELETE CASCADE,
    CONSTRAINT FK_PRI_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id)
);

-- 8. SALES, POS, QUOTATIONS & SALES RETURNS
CREATE TABLE SalesInvoices (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerId INT NOT NULL,
    WarehouseId INT NOT NULL,
    SaleDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SaleType NVARCHAR(30) NOT NULL DEFAULT 'POS',     -- 'POS', 'Wholesale', 'Online', 'Service'
    SubTotal DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DueAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    PaymentMethod NVARCHAR(50) NOT NULL DEFAULT 'Cash', -- 'Cash', 'Card', 'Bank', 'MobileBanking', 'Credit'
    PaymentStatus NVARCHAR(30) NOT NULL DEFAULT 'Paid',  -- 'Paid', 'Partial', 'Due'
    Status NVARCHAR(30) NOT NULL DEFAULT 'Completed',    -- 'Draft', 'Completed', 'Cancelled'
    Notes NVARCHAR(500) NULL,
    CreatedByUserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Sales_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_Sales_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id),
    CONSTRAINT FK_Sales_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE TABLE SalesItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SalesInvoiceId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    DiscountPercent DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_SalesItems_Invoice FOREIGN KEY (SalesInvoiceId) REFERENCES SalesInvoices(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SalesItems_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id)
);

CREATE TABLE SalesReturns (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ReturnNumber NVARCHAR(50) NOT NULL UNIQUE,
    SalesInvoiceId INT NULL,
    CustomerId INT NOT NULL,
    WarehouseId INT NOT NULL,
    ReturnDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    TotalRefundAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Reason NVARCHAR(255) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Completed',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_SR_SalesInvoice FOREIGN KEY (SalesInvoiceId) REFERENCES SalesInvoices(Id),
    CONSTRAINT FK_SR_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_SR_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id)
);

CREATE TABLE SalesReturnItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SalesReturnId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_SRI_SalesReturn FOREIGN KEY (SalesReturnId) REFERENCES SalesReturns(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SRI_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id)
);

CREATE TABLE Quotations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuotationNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerId INT NOT NULL,
    ExpiryDate DATETIME2 NULL,
    SubTotal DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Accepted', 'Rejected', 'Converted'
    Notes NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Quotations_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

CREATE TABLE QuotationItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuotationId INT NOT NULL,
    PartId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_QuotationItems_Quotation FOREIGN KEY (QuotationId) REFERENCES Quotations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_QuotationItems_Parts FOREIGN KEY (PartId) REFERENCES Parts(Id)
);

-- 9. ACCOUNTING (CHART OF ACCOUNTS, JOURNAL, LEDGER, EXPENSES, INCOME, ASSETS, LIABILITIES)
CREATE TABLE Accounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountCode NVARCHAR(20) NOT NULL UNIQUE,
    AccountName NVARCHAR(100) NOT NULL,
    AccountType NVARCHAR(30) NOT NULL, -- 'Asset', 'Liability', 'Equity', 'Revenue', 'Expense'
    ParentAccountId INT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Accounts_Parent FOREIGN KEY (ParentAccountId) REFERENCES Accounts(Id)
);

CREATE TABLE JournalEntries (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EntryNumber NVARCHAR(50) NOT NULL UNIQUE,
    EntryDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ReferenceNumber NVARCHAR(50) NULL, -- Linked Invoice / Payment
    Description NVARCHAR(500) NULL,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    CreatedByUserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_JournalEntries_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE TABLE JournalItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    JournalEntryId INT NOT NULL,
    AccountId INT NOT NULL,
    Debit DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Credit DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Description NVARCHAR(255) NULL,
    CONSTRAINT FK_JournalItems_Entry FOREIGN KEY (JournalEntryId) REFERENCES JournalEntries(Id) ON DELETE CASCADE,
    CONSTRAINT FK_JournalItems_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
);

CREATE TABLE ExpenseCategories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL
);

CREATE TABLE Expenses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ExpenseNumber NVARCHAR(50) NOT NULL UNIQUE,
    ExpenseCategoryId INT NOT NULL,
    AccountId INT NULL, -- Payment source account (e.g. Cash / Bank)
    Amount DECIMAL(18,2) NOT NULL,
    ExpenseDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PaymentMethod NVARCHAR(50) NOT NULL DEFAULT 'Cash',
    Reference NVARCHAR(100) NULL,
    Note NVARCHAR(500) NULL,
    CreatedByUserId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Expenses_Categories FOREIGN KEY (ExpenseCategoryId) REFERENCES ExpenseCategories(Id),
    CONSTRAINT FK_Expenses_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id),
    CONSTRAINT FK_Expenses_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE TABLE Incomes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IncomeNumber NVARCHAR(50) NOT NULL UNIQUE,
    Source NVARCHAR(100) NOT NULL,
    AccountId INT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    IncomeDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PaymentMethod NVARCHAR(50) NOT NULL DEFAULT 'Cash',
    Reference NVARCHAR(100) NULL,
    Note NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Incomes_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
);

CREATE TABLE Assets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AssetName NVARCHAR(150) NOT NULL,
    AssetCode NVARCHAR(50) NOT NULL UNIQUE,
    PurchaseDate DATETIME2 NOT NULL,
    PurchaseCost DECIMAL(18,2) NOT NULL,
    DepreciationRate DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    CurrentValue DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Liabilities (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    CreditorName NVARCHAR(150) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    DueDate DATETIME2 NULL,
    PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Active', -- 'Active', 'Settled'
    Notes NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- 10. SYSTEM & COMPANY SETTINGS
CREATE TABLE CompanySettings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(150) NOT NULL DEFAULT 'Ainan International Auto Parts System',
    ShortName NVARCHAR(50) NOT NULL DEFAULT 'AIAPS',
    Tagline NVARCHAR(255) NOT NULL DEFAULT 'Your Trusted Auto Parts Partner Worldwide',
    Email NVARCHAR(100) NOT NULL DEFAULT 'info@aiaps.com',
    Phone NVARCHAR(50) NOT NULL DEFAULT '+880 1700-000000',
    Address NVARCHAR(255) NOT NULL DEFAULT 'Dhaka, Bangladesh',
    Website NVARCHAR(100) NOT NULL DEFAULT 'www.aiaps.com',
    TaxId NVARCHAR(50) NULL,
    TradeLicenseId NVARCHAR(50) NULL,
    CurrencyCode NVARCHAR(10) NOT NULL DEFAULT 'BDT',
    CurrencySymbol NVARCHAR(10) NOT NULL DEFAULT '৳',
    LogoUrl NVARCHAR(255) NULL,
    FooterText NVARCHAR(255) NOT NULL DEFAULT 'Thank you for your business | AIAPS',
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- ============================================================================
-- SEED DATA
-- ============================================================================

-- Roles
INSERT INTO Roles (Name, Description) VALUES
('SuperAdmin', 'Complete system administrator with full access'),
('Admin', 'Operations manager with operational and reporting access'),
('SalesStaff', 'Cashier and sales counter personnel'),
('InventoryManager', 'Warehouse and inventory control personnel'),
('Accountant', 'Financial, ledger and expense management personnel');

-- Permissions
INSERT INTO Permissions (ModuleName, ActionName, Description) VALUES
('Dashboard', 'View', 'View Dashboard Analytics'),
('Parts', 'Create', 'Create New Parts'),
('Parts', 'Read', 'View Parts List and Details'),
('Parts', 'Update', 'Update Parts Information'),
('Parts', 'Delete', 'Delete Parts'),
('Inventory', 'Manage', 'Manage Stock and Warehouses'),
('Sales', 'Create', 'Create Sales and POS Invoices'),
('Sales', 'Read', 'View Sales Reports and History'),
('Purchase', 'Create', 'Create Purchase Invoices'),
('Purchase', 'Read', 'View Purchases and Stock Intake'),
('Accounting', 'Manage', 'Manage Chart of Accounts and Journal'),
('Reports', 'View', 'Export and View Business Reports'),
('Settings', 'Manage', 'Modify Company and System Settings');

-- Default Admin User (Password: Admin@123 -> SHA256 hashed / placeholder)
INSERT INTO Users (Username, Email, PasswordHash, FullName, Phone, IsActive) VALUES
('admin', 'admin@aiaps.com', '$2a$11$N4W6K4s5V8R/HqI5YVfAue3QyB6fQdGZ2Y6nL1vXJ7eUoO1zP9KWi', 'System Administrator', '+880 1700-000001', 1);

INSERT INTO UserRoles (UserId, RoleId) VALUES (1, 1);

-- Units
INSERT INTO Units (Name, ShortCode, IsActive) VALUES
('Pieces', 'Pcs', 1),
('Set', 'Set', 1),
('Box', 'Box', 1),
('Pair', 'Pair', 1),
('Liter', 'Ltr', 1),
('Meter', 'Mtr', 1),
('Kilogram', 'Kg', 1);

-- Categories
INSERT INTO Categories (Name, Code, Description) VALUES
('Engine Parts', 'ENG', 'Pistons, Valves, Gaskets, Timing Belts, Crankshafts'),
('Brake System', 'BRK', 'Brake Pads, Brake Shoes, Rotors, Calipers, Master Cylinders'),
('Suspension & Steering', 'SUS', 'Shock Absorbers, Control Arms, Ball Joints, Tie Rods'),
('Electrical & Lighting', 'ELE', 'Headlights, Alternators, Starters, Spark Plugs, Batteries'),
('Filtration', 'FLT', 'Oil Filters, Air Filters, Fuel Filters, Cabin Filters'),
('Transmission & Clutch', 'TRN', 'Clutch Plates, Pressure Plates, Gearbox Components'),
('Cooling & Heating', 'COL', 'Radiators, Water Pumps, Thermostats, Coolant'),
('Body & Exterior', 'BDY', 'Bumpers, Mirrors, Fenders, Grilles, Door Handles'),
('Lubricants & Fluids', 'LUB', 'Engine Oils, Transmission Fluid, Brake Fluid');

-- Brands
INSERT INTO Brands (Name, Country, Description) VALUES
('Bosch', 'Germany', 'High quality electrical, braking and ignition components'),
('Denso', 'Japan', 'Spark plugs, alternators, radiators, sensors'),
('NGK', 'Japan', 'World-leading spark plugs and glow plugs'),
('Toyota Genuine Parts', 'Japan', 'Original equipment manufacturer for Toyota'),
('Brembo', 'Italy', 'High performance braking systems and pads'),
('KYB', 'Japan', 'Premium shock absorbers and struts'),
('Mann-Filter', 'Germany', 'Premium filtration products'),
('Mobil 1', 'USA', 'Synthetic motor oils and lubricants'),
('AISIN', 'Japan', 'Clutch kits, water pumps, transmission parts');

-- Warehouses
INSERT INTO Warehouses (Name, Code, Location, ContactPerson, Phone, IsDefault, IsActive) VALUES
('Main Central Warehouse', 'WH-MAIN', 'Dhaka Hub - Tejgaon Industrial Area', 'Tareq Ahmed', '+880 1711-000001', 1, 1),
('Chittagong Port Warehouse', 'WH-CTG', 'Agrabad Commercial Area, Chittagong', 'Kamal Hossain', '+880 1711-000002', 0, 1),
('Uttara Showroom & Store', 'WH-UTT', 'Sector 3, Uttara, Dhaka', 'Farhan Kabir', '+880 1711-000003', 0, 1);

-- Vehicles
INSERT INTO Vehicles (Make, Model, YearStart, YearEnd, Engine, FuelType) VALUES
('Toyota', 'Corolla', 2014, 2022, '1.8L 2ZR-FE', 'Petrol'),
('Toyota', 'Premio / Allion', 2012, 2021, '1.5L 1NZ-FE', 'Petrol'),
('Toyota', 'HiAce', 2010, 2024, '3.0L 1KD-FTV', 'Diesel'),
('Honda', 'Civic', 2016, 2022, '1.5L Turbo', 'Petrol'),
('Honda', 'Vezel / HR-V', 2014, 2021, '1.5L i-VTEC Hybrid', 'Hybrid'),
('Nissan', 'X-Trail', 2015, 2022, '2.0L MR20DD', 'Petrol'),
('Mitsubishi', 'Pajero Sport', 2016, 2024, '2.4L 4N15 MIVEC', 'Diesel'),
('Hyundai', 'Tucson', 2016, 2023, '2.0L Nu MPI', 'Petrol');

-- Sample Customers
INSERT INTO Customers (Name, CustomerType, Phone, Email, Address, CreditLimit, OpeningBalance, CurrentBalance) VALUES
('Apex Auto Garage', 'Garage', '+880 1812-345678', 'apex@autogarage.com', 'Mirpur-10, Dhaka', 150000.00, 0.00, 24500.00),
('Rahim Motors Wholesale', 'Wholesale', '+880 1913-987654', 'rahim@motorsbd.com', 'Dholaikhal, Old Dhaka', 500000.00, 0.00, 85000.00),
('Kabir Express Fleet', 'Corporate', '+880 1715-112233', 'fleet@kabirexpress.com', 'Mohakhali, Dhaka', 300000.00, 0.00, 0.00),
('Walk-in Customer (Counter)', 'Retail', '+880 1700-000000', 'retail@aiaps.com', 'Store Counter', 0.00, 0.00, 0.00);

-- Sample Suppliers
INSERT INTO Suppliers (Name, Company, Phone, Email, Address, OpeningBalance, CurrentBalance) VALUES
('Global Auto Parts Trading Dubai', 'Global Parts FZE', '+971 4 1234567', 'sales@globalpartsfze.ae', 'Al Quoz Industrial, Dubai, UAE', 0.00, 120000.00),
('Nippon Auto Exports Tokyo', 'Nippon Exports Co.', '+81 3 55556666', 'orders@nipponexports.jp', 'Yokohama, Japan', 0.00, 350000.00),
('Bangla Motor Spares Importers', 'BMS Importers Ltd.', '+880 2 9887766', 'info@bmsimporters.com', 'Motijheel, Dhaka', 0.00, 45000.00);

-- Sample Parts
INSERT INTO Parts (PartNumber, OEMNumber, Barcode, Name, Description, CategoryId, BrandId, UnitId, CostPrice, SellingPrice, WholesalePrice, MinStockAlert) VALUES
('BOS-BP-001', '04465-02220', '890123450001', 'Front Ceramic Brake Pad Set', 'Low dust high performance ceramic brake pads for Toyota Corolla / Premio', 2, 1, 4, 2200.00, 3200.00, 2700.00, 10),
('DEN-SP-IX01', '90919-01210', '890123450002', 'Iridium Power Spark Plug (SK20R11)', 'High ignition efficiency laser iridium spark plug', 4, 2, 1, 650.00, 950.00, 800.00, 30),
('NGK-SP-BKR6', '98079-5614E', '890123450003', 'NGK Laser Platinum Spark Plug', 'Long life platinum electrode for Honda Civic / Vezel', 4, 3, 1, 550.00, 850.00, 700.00, 25),
('MNN-OF-W68', '90915-YZZE1', '890123450004', 'Mann Spin-On Engine Oil Filter', 'Micro-fiber high dirt holding oil filter for Japanese sedans', 5, 7, 1, 400.00, 650.00, 520.00, 20),
('KYB-SA-33331', '48510-80255', '890123450005', 'KYB Excel-G Front Strut Assembly (Right)', 'Twin tube gas strut for smooth comfort and stability', 3, 6, 1, 6500.00, 9200.00, 7800.00, 4),
('MOB-OIL-5W30', 'MOBIL-SYN-4L', '890123450006', 'Mobil 1 Fully Synthetic Engine Oil 5W-30 (4L Can)', 'Advanced full synthetic formula with ultimate protection', 9, 8, 1, 3800.00, 5200.00, 4400.00, 15),
('AIS-CK-TY01', '31250-12390', '890123450007', 'AISIN Clutch Plate & Pressure Plate Kit', 'Heavy duty clutch assembly for Toyota HiAce', 6, 9, 2, 12500.00, 16800.00, 14200.00, 3);

-- Stock Distribution in Main Warehouse
INSERT INTO WarehouseStocks (WarehouseId, PartId, Quantity, RackLocation, BinLocation) VALUES
(1, 1, 45, 'Rack-A1', 'Bin-04'),
(1, 2, 120, 'Rack-B2', 'Bin-12'),
(1, 3, 80, 'Rack-B2', 'Bin-14'),
(1, 4, 95, 'Rack-C1', 'Bin-02'),
(1, 5, 12, 'Rack-D3', 'Bin-01'),
(1, 6, 38, 'Rack-E1', 'Bin-08'),
(1, 7, 8, 'Rack-F2', 'Bin-03'),
(2, 1, 20, 'Rack-CTG-1', 'Bin-A'),
(2, 6, 15, 'Rack-CTG-3', 'Bin-C');

-- Chart of Accounts
INSERT INTO Accounts (AccountCode, AccountName, AccountType, ParentAccountId, Balance) VALUES
('1010', 'Cash on Hand', 'Asset', NULL, 150000.00),
('1020', 'City Bank - Corporate Account', 'Asset', NULL, 850000.00),
('1030', 'Accounts Receivable (Customers)', 'Asset', NULL, 109500.00),
('1040', 'Merchandise Inventory', 'Asset', NULL, 780000.00),
('2010', 'Accounts Payable (Suppliers)', 'Liability', NULL, 515000.00),
('2020', 'Short Term Loan / Credit', 'Liability', NULL, 200000.00),
('3010', 'Owner Capital / Equity', 'Equity', NULL, 1000000.00),
('4010', 'Auto Parts Sales Revenue', 'Revenue', NULL, 0.00),
('4020', 'Service & Labor Income', 'Revenue', NULL, 0.00),
('5010', 'Cost of Goods Sold (COGS)', 'Expense', NULL, 0.00),
('5020', 'Showroom & Warehouse Rent', 'Expense', NULL, 0.00),
('5030', 'Staff Salaries & Benefits', 'Expense', NULL, 0.00),
('5040', 'Electricity & Utilities', 'Expense', NULL, 0.00),
('5050', 'Logistics & Freight Expense', 'Expense', NULL, 0.00);

-- Expense Categories
INSERT INTO ExpenseCategories (Name, Description) VALUES
('Rent & Facilities', 'Office, warehouse and showroom rent'),
('Salaries & Payroll', 'Staff monthly wages and incentives'),
('Utilities & Bills', 'Electricity, Internet, Water and Generator Fuel'),
('Transport & Shipping', 'Freight intake, port handling and local delivery'),
('Maintenance & Office', 'Store maintenance, tea, printing and stationery');

-- Company Settings
INSERT INTO CompanySettings (CompanyName, ShortName, Tagline, Email, Phone, Address, Website, CurrencyCode, CurrencySymbol, FooterText) VALUES
('Ainan International Auto Parts System', 'AIAPS', 'Your Trusted Auto Parts Partner Worldwide', 'info@aiaps.com', '+880 1700-000000', 'Tejgaon Industrial Area, Dhaka, Bangladesh', 'www.aiaps.com', 'BDT', '৳', 'Thank you for your business | AIAPS');
GO
