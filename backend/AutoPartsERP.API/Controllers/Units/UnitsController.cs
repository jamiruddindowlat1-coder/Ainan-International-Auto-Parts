using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Catalog;

namespace AutoPartsERP.API.Controllers.Units;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly AppDbContext _context;

    public UnitsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UnitDto>>>> GetUnits([FromQuery] string? search)
    {
        var query = _context.Units.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(u => u.Name.ToLower().Contains(s) || u.ShortCode.ToLower().Contains(s));
        }

        var list = await query
            .OrderBy(u => u.Name)
            .Select(u => new UnitDto
            {
                Id = u.Id,
                Name = u.Name,
                ShortCode = u.ShortCode,
                IsActive = u.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<UnitDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UnitDto>>> GetUnitById(int id)
    {
        var u = await _context.Units.FindAsync(id);
        if (u == null) return NotFound(ApiResponse<UnitDto>.Fail("Unit not found"));

        return Ok(ApiResponse<UnitDto>.Ok(new UnitDto
            {
                Id = u.Id,
                Name = u.Name,
                ShortCode = u.ShortCode,
                IsActive = u.IsActive
            }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UnitDto>>> CreateUnit([FromBody] UnitDto dto)
    {
        var unit = new Unit
        {
            Name = dto.Name,
            ShortCode = dto.ShortCode,
            IsActive = dto.IsActive
        };

        await _context.Units.AddAsync(unit);
        await _context.SaveChangesAsync();

        dto.Id = unit.Id;
        return CreatedAtAction(nameof(GetUnitById), new { id = unit.Id }, ApiResponse<UnitDto>.Ok(dto, "Unit created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UnitDto>>> UpdateUnit(int id, [FromBody] UnitDto dto)
    {
        var unit = await _context.Units.FindAsync(id);
        if (unit == null) return NotFound(ApiResponse<UnitDto>.Fail("Unit not found"));

        unit.Name = dto.Name;
        unit.ShortCode = dto.ShortCode;
        unit.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<UnitDto>.Ok(dto, "Unit updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUnit(int id)
    {
        var unit = await _context.Units.Include(u => u.Parts).FirstOrDefaultAsync(u => u.Id == id);
        if (unit == null) return NotFound(ApiResponse<bool>.Fail("Unit not found"));

        if (unit.Parts.Any())
        {
            return BadRequest(ApiResponse<bool>.Fail("Cannot delete unit because it has associated parts."));
        }

        _context.Units.Remove(unit);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Unit deleted successfully"));
    }
}

