using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Controllers.Inventory;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("warehouses")]
    public async Task<ActionResult<ApiResponse<List<Warehouse>>>> GetWarehouses()
    {
        var list = await _context.Warehouses.AsNoTracking().ToListAsync();
        return Ok(ApiResponse<List<Warehouse>>.Ok(list));
    }

    [HttpGet("stocks")]
    public async Task<ActionResult<ApiResponse<List<object>>>> GetStockLevels([FromQuery] int? warehouseId)
    {
        var query = _context.WarehouseStocks
            .Include(ws => ws.Warehouse)
            .Include(ws => ws.Part)
            .AsNoTracking()
            .AsQueryable();

        if (warehouseId.HasValue)
            query = query.Where(ws => ws.WarehouseId == warehouseId.Value);

        var list = await query
            .Select(ws => new
            {
                ws.Id,
                ws.WarehouseId,
                WarehouseName = ws.Warehouse.Name,
                ws.PartId,
                PartNumber = ws.Part.PartNumber,
                PartName = ws.Part.Name,
                ws.Quantity,
                ws.RackLocation,
                ws.BinLocation,
                ws.LastUpdated
            })
            .ToListAsync();

        return Ok(ApiResponse<List<object>>.Ok(list.Cast<object>().ToList()));
    }

    [HttpGet("movements")]
    public async Task<ActionResult<ApiResponse<List<object>>>> GetStockMovements([FromQuery] int? partId, [FromQuery] int? warehouseId)
    {
        var query = _context.StockMovements
            .Include(sm => sm.Part)
            .Include(sm => sm.Warehouse)
            .AsNoTracking()
            .AsQueryable();

        if (partId.HasValue) query = query.Where(sm => sm.PartId == partId.Value);
        if (warehouseId.HasValue) query = query.Where(sm => sm.WarehouseId == warehouseId.Value);

        var list = await query
            .OrderByDescending(sm => sm.CreatedAt)
            .Take(50)
            .Select(sm => new
            {
                sm.Id,
                PartNumber = sm.Part.PartNumber,
                PartName = sm.Part.Name,
                WarehouseName = sm.Warehouse.Name,
                sm.MovementType,
                sm.Quantity,
                sm.UnitCost,
                sm.ReferenceNumber,
                sm.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<List<object>>.Ok(list.Cast<object>().ToList()));
    }
}
