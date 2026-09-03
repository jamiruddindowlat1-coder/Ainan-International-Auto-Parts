using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Partners;

namespace AutoPartsERP.API.Controllers.Suppliers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SupplierDto>>>> GetSuppliers([FromQuery] string? search)
    {
        var query = _context.Suppliers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(x => x.Name.ToLower().Contains(s) || (x.Company != null && x.Company.ToLower().Contains(s)) || x.Phone.Contains(s));
        }

        var list = await query
            .OrderBy(x => x.Name)
            .Select(x => new SupplierDto
            {
                Id = x.Id,
                Name = x.Name,
                Company = x.Company,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                TaxNumber = x.TaxNumber,
                CurrentBalance = x.CurrentBalance,
                IsActive = x.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<SupplierDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> GetSupplierById(int id)
    {
        var s = await _context.Suppliers.FindAsync(id);
        if (s == null) return NotFound(ApiResponse<SupplierDto>.Fail("Supplier not found"));

        return Ok(ApiResponse<SupplierDto>.Ok(new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Company = s.Company,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            TaxNumber = s.TaxNumber,
            CurrentBalance = s.CurrentBalance,
            IsActive = s.IsActive
        }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SupplierDto>>> CreateSupplier([FromBody] SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            Company = dto.Company,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            TaxNumber = dto.TaxNumber,
            OpeningBalance = 0,
            CurrentBalance = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        dto.Id = supplier.Id;
        return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, ApiResponse<SupplierDto>.Ok(dto, "Supplier created successfully"));
    }
}
