using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Catalog;

namespace AutoPartsERP.API.Services;

public class PartsService : IPartsService
{
    private readonly AppDbContext _context;

    public PartsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<PartDto>> GetPartsAsync(string? search, int? categoryId, int? brandId, int page = 1, int pageSize = 20)
    {
        var query = _context.Parts
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Unit)
            .Include(p => p.WarehouseStocks)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(p =>
                p.PartNumber.ToLower().Contains(s) ||
                p.Name.ToLower().Contains(s) ||
                (p.OEMNumber != null && p.OEMNumber.ToLower().Contains(s)) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(s)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PartDto
            {
                Id = p.Id,
                PartNumber = p.PartNumber,
                OEMNumber = p.OEMNumber,
                Barcode = p.Barcode,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand.Name,
                UnitId = p.UnitId,
                UnitName = p.Unit.ShortCode,
                CostPrice = p.CostPrice,
                SellingPrice = p.SellingPrice,
                WholesalePrice = p.WholesalePrice,
                MinStockAlert = p.MinStockAlert,
                TotalStock = p.WarehouseStocks.Sum(ws => ws.Quantity),
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive
            })
            .ToListAsync();

        return new PagedResponse<PartDto>
        {
            Success = true,
            Message = "Parts retrieved successfully",
            Data = items,
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ApiResponse<PartDto>> GetPartByIdAsync(int id)
    {
        var p = await _context.Parts
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Unit)
            .Include(p => p.WarehouseStocks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p == null) return ApiResponse<PartDto>.Fail("Part not found");

        return ApiResponse<PartDto>.Ok(new PartDto
        {
            Id = p.Id,
            PartNumber = p.PartNumber,
            OEMNumber = p.OEMNumber,
            Barcode = p.Barcode,
            Name = p.Name,
            Description = p.Description,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name,
            BrandId = p.BrandId,
            BrandName = p.Brand?.Name,
            UnitId = p.UnitId,
            UnitName = p.Unit?.ShortCode,
            CostPrice = p.CostPrice,
            SellingPrice = p.SellingPrice,
            WholesalePrice = p.WholesalePrice,
            MinStockAlert = p.MinStockAlert,
            TotalStock = p.WarehouseStocks.Sum(ws => ws.Quantity),
            ImageUrl = p.ImageUrl,
            IsActive = p.IsActive
        });
    }

    public async Task<ApiResponse<PartDto>> CreatePartAsync(CreatePartDto dto)
    {
        if (await _context.Parts.AnyAsync(p => p.PartNumber.ToLower() == dto.PartNumber.ToLower()))
        {
            return ApiResponse<PartDto>.Fail("Part number already exists");
        }

        var part = new Part
        {
            PartNumber = dto.PartNumber.Trim(),
            OEMNumber = dto.OEMNumber?.Trim(),
            Barcode = dto.Barcode?.Trim(),
            Name = dto.Name.Trim(),
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            UnitId = dto.UnitId,
            CostPrice = dto.CostPrice,
            SellingPrice = dto.SellingPrice,
            WholesalePrice = dto.WholesalePrice,
            MinStockAlert = dto.MinStockAlert,
            ImageUrl = dto.ImageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Parts.AddAsync(part);
        await _context.SaveChangesAsync();

        return await GetPartByIdAsync(part.Id);
    }

    public async Task<ApiResponse<PartDto>> UpdatePartAsync(int id, CreatePartDto dto)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part == null) return ApiResponse<PartDto>.Fail("Part not found");

        part.PartNumber = dto.PartNumber.Trim();
        part.OEMNumber = dto.OEMNumber?.Trim();
        part.Barcode = dto.Barcode?.Trim();
        part.Name = dto.Name.Trim();
        part.Description = dto.Description;
        part.CategoryId = dto.CategoryId;
        part.BrandId = dto.BrandId;
        part.UnitId = dto.UnitId;
        part.CostPrice = dto.CostPrice;
        part.SellingPrice = dto.SellingPrice;
        part.WholesalePrice = dto.WholesalePrice;
        part.MinStockAlert = dto.MinStockAlert;
        part.ImageUrl = dto.ImageUrl;
        part.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetPartByIdAsync(part.Id);
    }

    public async Task<ApiResponse<bool>> DeletePartAsync(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part == null) return ApiResponse<bool>.Fail("Part not found");

        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Part deleted successfully");
    }

    public async Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync()
    {
        var list = await _context.Categories
            .Include(c => c.Parts)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                PartsCount = c.Parts.Count,
                IsActive = c.IsActive
            })
            .ToListAsync();

        return ApiResponse<List<CategoryDto>>.Ok(list);
    }

    public async Task<ApiResponse<List<BrandDto>>> GetBrandsAsync()
    {
        var list = await _context.Brands
            .Include(b => b.Parts)
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Country = b.Country,
                Description = b.Description,
                LogoUrl = b.LogoUrl,
                PartsCount = b.Parts.Count,
                IsActive = b.IsActive
            })
            .ToListAsync();

        return ApiResponse<List<BrandDto>>.Ok(list);
    }

    public async Task<ApiResponse<List<UnitDto>>> GetUnitsAsync()
    {
        var list = await _context.Units
            .Select(u => new UnitDto
            {
                Id = u.Id,
                Name = u.Name,
                ShortCode = u.ShortCode,
                IsActive = u.IsActive
            })
            .ToListAsync();

        return ApiResponse<List<UnitDto>>.Ok(list);
    }
}
