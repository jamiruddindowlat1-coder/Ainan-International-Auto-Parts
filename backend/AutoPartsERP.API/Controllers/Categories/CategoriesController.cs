using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Catalog;

namespace AutoPartsERP.API.Controllers.Categories;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetCategories([FromQuery] string? search)
    {
        var query = _context.Categories.Include(c => c.Parts).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(c => c.Name.ToLower().Contains(s));
        }

        var list = await query
            .OrderBy(c => c.Name)
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

        return Ok(ApiResponse<List<CategoryDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(int id)
    {
        var c = await _context.Categories.Include(x => x.Parts).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound(ApiResponse<CategoryDto>.Fail("Category not found"));

        return Ok(ApiResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                PartsCount = c.Parts.Count,
                IsActive = c.IsActive
            }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Code = dto.Code,
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        dto.Id = category.Id;
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, ApiResponse<CategoryDto>.Ok(dto, "Category created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(int id, [FromBody] CategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound(ApiResponse<CategoryDto>.Fail("Category not found"));

        category.Name = dto.Name;
        category.Code = dto.Code;
        category.Description = dto.Description;
        category.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<CategoryDto>.Ok(dto, "Category updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
    {
        var category = await _context.Categories.Include(c => c.Parts).FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return NotFound(ApiResponse<bool>.Fail("Category not found"));

        if (category.Parts.Any())
        {
            return BadRequest(ApiResponse<bool>.Fail("Cannot delete category because it has associated parts."));
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Category deleted successfully"));
    }
}

