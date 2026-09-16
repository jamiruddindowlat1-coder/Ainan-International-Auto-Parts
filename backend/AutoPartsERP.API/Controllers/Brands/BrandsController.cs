using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Catalog;

namespace AutoPartsERP.API.Controllers.Brands;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BrandsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BrandDto>>>> GetBrands([FromQuery] string? search)
    {
        var query = _context.Brands.Include(b => b.Parts).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(b => b.Name.ToLower().Contains(s));
        }

        var list = await query
            .OrderBy(b => b.Name)
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

        return Ok(ApiResponse<List<BrandDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BrandDto>>> GetBrandById(int id)
    {
        var b = await _context.Brands.Include(x => x.Parts).FirstOrDefaultAsync(x => x.Id == id);
        if (b == null) return NotFound(ApiResponse<BrandDto>.Fail("Brand not found"));

        return Ok(ApiResponse<BrandDto>.Ok(new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Country = b.Country,
                Description = b.Description,
                LogoUrl = b.LogoUrl,
                PartsCount = b.Parts.Count,
                IsActive = b.IsActive
            }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BrandDto>>> CreateBrand([FromBody] BrandDto dto)
    {
        var brand = new Brand
        {
            Name = dto.Name,
            Country = dto.Country,
            Description = dto.Description,
            LogoUrl = dto.LogoUrl,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();

        dto.Id = brand.Id;
        return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, ApiResponse<BrandDto>.Ok(dto, "Brand created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<BrandDto>>> UpdateBrand(int id, [FromBody] BrandDto dto)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null) return NotFound(ApiResponse<BrandDto>.Fail("Brand not found"));

        brand.Name = dto.Name;
        brand.Country = dto.Country;
        brand.Description = dto.Description;
        brand.LogoUrl = dto.LogoUrl;
        brand.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<BrandDto>.Ok(dto, "Brand updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBrand(int id)
    {
        var brand = await _context.Brands.Include(b => b.Parts).FirstOrDefaultAsync(b => b.Id == id);
        if (brand == null) return NotFound(ApiResponse<bool>.Fail("Brand not found"));

        if (brand.Parts.Any())
        {
            return BadRequest(ApiResponse<bool>.Fail("Cannot delete brand because it has associated parts."));
        }

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Brand deleted successfully"));
    }
}

