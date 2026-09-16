using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Purchase;

namespace AutoPartsERP.API.Controllers.PurchaseReturns;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PurchaseReturnsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PurchaseReturnsController(AppDbContext context)
    {
        _context = context;
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
    public async Task<ActionResult<ApiResponse<PurchaseReturnDto>>> CreatePurchaseReturn([FromBody] PurchaseReturnDto dto)
    {
        var pr = new PurchaseReturn
        {
            ReturnNumber = string.IsNullOrWhiteSpace(dto.ReturnNumber) ? "PR-" + DateTime.Now.Ticks : dto.ReturnNumber,
            SupplierId = dto.SupplierId,
            TotalRefundAmount = dto.TotalAmount,
            WarehouseId = 1, // default for demo

            ReturnDate = DateTime.UtcNow
        };
        await _context.PurchaseReturns.AddAsync(pr);
        await _context.SaveChangesAsync();
        dto.Id = pr.Id;
        return Ok(ApiResponse<PurchaseReturnDto>.Ok(dto));
    }
}

