using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Partners;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPartnerService _service;

    public CustomersController(AppDbContext context, IPartnerService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CustomerDto>>>> GetCustomers([FromQuery] string? search)
    {
        var query = _context.Customers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(c => c.Name.ToLower().Contains(s) || c.Phone.Contains(s) || (c.Email != null && c.Email.ToLower().Contains(s)));
        }

        var list = await query
            .OrderBy(c => c.Name)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                CustomerType = c.CustomerType,
                Phone = c.Phone,
                Email = c.Email,
                Address = c.Address,
                CreditLimit = c.CreditLimit,
                CurrentBalance = c.CurrentBalance,
                IsActive = c.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<CustomerDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerById(int id)
    {
        var c = await _context.Customers.FindAsync(id);
        if (c == null) return NotFound(ApiResponse<CustomerDto>.Fail("Customer not found"));

        return Ok(ApiResponse<CustomerDto>.Ok(new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            CustomerType = c.CustomerType,
            Phone = c.Phone,
            Email = c.Email,
            Address = c.Address,
            CreditLimit = c.CreditLimit,
            CurrentBalance = c.CurrentBalance,
            IsActive = c.IsActive
        }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer([FromBody] CustomerDto dto)
    {
        var result = await _service.CreateCustomerAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetCustomerById), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(int id, [FromBody] CustomerDto dto)
    {
        if (!await _context.Customers.AnyAsync(c => c.Id == id))
            return NotFound(ApiResponse<CustomerDto>.Fail("Customer not found"));

        var result = await _service.UpdateCustomerAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}