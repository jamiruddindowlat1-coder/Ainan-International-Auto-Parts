using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Warehouses;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWarehouseService _service;

    public WarehousesController(AppDbContext context, IWarehouseService service)
    {
        _context = context;
        _service = service;
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
        var result = await _service.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> UpdateWarehouse(int id, [FromBody] WarehouseDto dto)
    {
        if (!await _context.Warehouses.AnyAsync(w => w.Id == id))
            return NotFound(ApiResponse<WarehouseDto>.Fail("Warehouse not found"));

        var result = await _service.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}