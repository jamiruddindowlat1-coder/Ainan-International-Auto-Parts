using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Catalog;

namespace AutoPartsERP.API.Controllers.Vehicles;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly AppDbContext _context;

    public VehiclesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VehicleDto>>>> GetVehicles()
    {
        var list = await _context.Vehicles.AsNoTracking()
            .Select(v => new VehicleDto
            {
                Id = v.Id,
                Make = v.Make,
                Model = v.Model,
                YearStart = v.YearStart,
                YearEnd = v.YearEnd,
                Engine = v.Engine,
                FuelType = v.FuelType
            }).ToListAsync();
        return Ok(ApiResponse<List<VehicleDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> CreateVehicle([FromBody] VehicleDto dto)
    {
        var vehicle = new Vehicle
        {
            Make = dto.Make,
            Model = dto.Model,
            YearStart = dto.YearStart,
            YearEnd = dto.YearEnd,
            Engine = dto.Engine,
            FuelType = dto.FuelType,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        dto.Id = vehicle.Id;
        return Ok(ApiResponse<VehicleDto>.Ok(dto));
    }
}

