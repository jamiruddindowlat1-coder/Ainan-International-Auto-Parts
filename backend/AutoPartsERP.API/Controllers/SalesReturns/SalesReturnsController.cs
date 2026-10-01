using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.SalesReturns;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesReturnsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ISalesReturnService _service;

    public SalesReturnsController(AppDbContext context, ISalesReturnService service)
    {
        _context = context;
        _service = service;
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
    public async Task<ActionResult<ApiResponse<SalesReturnResultDto>>> CreateSalesReturn([FromBody] CreateSalesReturnDto dto)
    {
        int? userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
        var result = await _service.CreateAsync(dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}