using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Partners;

namespace AutoPartsERP.API.Controllers.Partners;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
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
        var customer = new Customer
        {
            Name = dto.Name,
            CustomerType = dto.CustomerType,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            CreditLimit = dto.CreditLimit,
            OpeningBalance = 0,
            CurrentBalance = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        dto.Id = customer.Id;
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, ApiResponse<CustomerDto>.Ok(dto, "Customer created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(int id, [FromBody] CustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound(ApiResponse<CustomerDto>.Fail("Customer not found"));

        customer.Name = dto.Name;
        customer.CustomerType = dto.CustomerType;
        customer.Phone = dto.Phone;
        customer.Email = dto.Email;
        customer.Address = dto.Address;
        customer.CreditLimit = dto.CreditLimit;
        customer.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<CustomerDto>.Ok(dto, "Customer updated successfully"));
    }
}

