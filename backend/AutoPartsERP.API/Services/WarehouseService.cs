using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Inventory;

namespace AutoPartsERP.API.Services;

public class WarehouseService : IWarehouseService
{
    private readonly AppDbContext _context;

    public WarehouseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<WarehouseDto>> CreateAsync(WarehouseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return ApiResponse<WarehouseDto>.Fail("Name is required");

        if (string.IsNullOrWhiteSpace(dto.Code))
            return ApiResponse<WarehouseDto>.Fail("Code is required");

        var code = dto.Code.Trim();
        if (await CodeTakenAsync(code, 0))
            return ApiResponse<WarehouseDto>.Fail($"Warehouse code '{code}' is already in use");

        var warehouse = new Warehouse
        {
            Name = dto.Name.Trim(),
            Code = code,
            Location = Clean(dto.Location),
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Warehouses.AddAsync(warehouse);
        await _context.SaveChangesAsync();

        return ApiResponse<WarehouseDto>.Ok(ToDto(warehouse), "Warehouse created successfully");
    }

    public async Task<ApiResponse<WarehouseDto>> UpdateAsync(int id, WarehouseDto dto)
    {
        var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == id);
        if (warehouse == null)
            return ApiResponse<WarehouseDto>.Fail("Warehouse not found");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return ApiResponse<WarehouseDto>.Fail("Name is required");

        if (string.IsNullOrWhiteSpace(dto.Code))
            return ApiResponse<WarehouseDto>.Fail("Code is required");

        var code = dto.Code.Trim();
        if (await CodeTakenAsync(code, id))
            return ApiResponse<WarehouseDto>.Fail($"Warehouse code '{code}' is already in use");

        if (!dto.IsActive && warehouse.IsActive)
        {
            if (warehouse.IsDefault)
                return ApiResponse<WarehouseDto>.Fail("Cannot deactivate the default warehouse");

            var stock = await _context.WarehouseStocks
                .Where(ws => ws.WarehouseId == id)
                .SumAsync(ws => (int?)ws.Quantity) ?? 0;

            if (stock != 0)
                return ApiResponse<WarehouseDto>.Fail("Cannot deactivate a warehouse that still holds stock");
        }

        warehouse.Name = dto.Name.Trim();
        warehouse.Code = code;
        warehouse.Location = Clean(dto.Location);
        warehouse.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return ApiResponse<WarehouseDto>.Ok(ToDto(warehouse), "Warehouse updated successfully");
    }

    private Task<bool> CodeTakenAsync(string code, int exceptId) =>
        _context.Warehouses.AnyAsync(w => w.Id != exceptId && w.Code.ToLower() == code.ToLower());

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static WarehouseDto ToDto(Warehouse w) => new()
    {
        Id = w.Id,
        Name = w.Name,
        Code = w.Code,
        Location = w.Location,
        IsActive = w.IsActive
    };
}