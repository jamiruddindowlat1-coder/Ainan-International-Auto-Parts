using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Sales;

namespace AutoPartsERP.API.Controllers.SalesReturns;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesReturnsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesReturnsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalesReturnDto>>>> GetSalesReturns()
    {
        var list = await _context.SalesReturns.AsNoTracking()
            .Select(sr => new SalesReturnDto
            {
                Id = sr.Id,
                ReturnNumber = sr.ReturnNumber,
                CustomerId = sr.CustomerId,
                TotalAmount = sr.TotalRefundAmount
            }).ToListAsync();
        return Ok(ApiResponse<List<SalesReturnDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesReturnDto>>> CreateSalesReturn([FromBody] SalesReturnDto dto)
    {
        var sr = new SalesReturn
        {
            ReturnNumber = string.IsNullOrWhiteSpace(dto.ReturnNumber) ? "SR-" + DateTime.Now.Ticks : dto.ReturnNumber,
            CustomerId = dto.CustomerId,
            TotalRefundAmount = dto.TotalAmount,
            WarehouseId = 1, // default
            ReturnDate = DateTime.UtcNow
        };
        await _context.SalesReturns.AddAsync(sr);
        await _context.SaveChangesAsync();
        dto.Id = sr.Id;
        return Ok(ApiResponse<SalesReturnDto>.Ok(dto));
    }
}

