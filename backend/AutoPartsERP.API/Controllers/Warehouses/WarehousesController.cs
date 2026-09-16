using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Controllers.Warehouses;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WarehousesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<WarehouseDto>>>> GetWarehouses()
    {
        var list = await _context.Warehouses.AsNoTracking()
            .Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Code = w.Code,
                Location = w.Location,
                IsActive = w.IsActive
            }).ToListAsync();
        return Ok(ApiResponse<List<WarehouseDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> CreateWarehouse([FromBody] WarehouseDto dto)
    {
        var warehouse = new Warehouse
        {
            Name = dto.Name,
            Code = dto.Code,
            Location = dto.Location,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Warehouses.AddAsync(warehouse);
        await _context.SaveChangesAsync();
        dto.Id = warehouse.Id;
        return Ok(ApiResponse<WarehouseDto>.Ok(dto));
    }
}

