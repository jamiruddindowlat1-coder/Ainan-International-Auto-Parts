using Microsoft.AspNetCore.Mvc;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Purchase;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<PurchaseInvoiceDto>>> GetPurchases(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await _purchaseService.GetPurchasesAsync(search, status, fromDate, toDate, page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> GetPurchaseById(int id)
    {
        var result = await _purchaseService.GetPurchaseByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> CreatePurchase([FromBody] CreatePurchaseInvoiceDto dto)
    {
        var result = await _purchaseService.CreatePurchaseAsync(dto, null);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetPurchaseById), new { id = result.Data!.Id }, result);
    }
}
