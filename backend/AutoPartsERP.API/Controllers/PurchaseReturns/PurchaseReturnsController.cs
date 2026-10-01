using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.PurchaseReturns;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PurchaseReturnsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPurchaseReturnService _service;

    public PurchaseReturnsController(AppDbContext context, IPurchaseReturnService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PurchaseReturnDto>>>> GetPurchaseReturns()
    {
        var list = await _context.PurchaseReturns.AsNoTracking()
            .Select(pr => new PurchaseReturnDto
            {
                Id = pr.Id,
                ReturnNumber = pr.ReturnNumber,
                SupplierId = pr.SupplierId,
                TotalAmount = pr.TotalRefundAmount
            }).ToListAsync();
        return Ok(ApiResponse<List<PurchaseReturnDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseReturnResultDto>>> CreatePurchaseReturn([FromBody] CreatePurchaseReturnDto dto)
    {
        int? userId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
        var result = await _service.CreateAsync(dto, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}