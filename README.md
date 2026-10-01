![CI](https://github.com/jamiruddindowlat1-coder/Ainan-International-Auto-Parts/actions/workflows/ci.yml/badge.svg)

# Ainan International Auto Parts System (AIAPS)

A full-stack POS and ERP platform for auto parts retailers, wholesalers and importers, built with ASP.NET Core (.NET 8), React and SQL Server.

## Showcase

- [Watch the portfolio video](https://cdn.jsdelivr.net/gh/jamiruddindowlat1-coder/Ainan-International-Auto-Parts@main/showcase/AIAPS_Portfolio_Video.mp4)
- [View the project showcase PDF](https://cdn.jsdelivr.net/gh/jamiruddindowlat1-coder/Ainan-International-Auto-Parts@main/showcase/AIAPS.pdf)

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8), Entity Framework Core, SQL Server, JWT Authentication, BCrypt, Swagger/OpenAPI
- **Frontend:** React 18, Vite, React Router, Recharts, Axios

## Modules

- Point of Sale (POS)
- Inventory, Brands, Categories, Units and Vehicles
- Sales, Quotations and Sales Returns
- Purchasing, Suppliers and Purchase Returns
- Multi-warehouse management
- Accounting: Journal, Ledger, Assets, Liabilities, Income and Expenses
- Reports and Dashboard with sales charts
- Users, Roles and Permissions

## Getting Started

### Database

Run `backend/database/schema_and_seed.sql` in SQL Server to create the tables and demo seed data.

### Backend

```bash
cd backend/AutoPartsERP.API
copy appsettings.json.example appsettings.json
# update ConnectionStrings and JwtSettings:Secret in appsettings.json
dotnet restore
dotnet run
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

## Security Note

`appsettings.json` (containing local connection strings and the JWT secret) is excluded from version control. Use `appsettings.json.example` as a template and supply your own values.

## Author

**Mohammed Jamir Uddin**
Full-Stack Software Developer (.NET Core and React)
jamiruddindowlat1@gmail.com

