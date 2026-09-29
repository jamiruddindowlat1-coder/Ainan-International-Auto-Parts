using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Sales;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<SalesInvoiceDto>>> GetSales(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await _salesService.GetSalesAsync(search, status, fromDate, toDate, page, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SalesInvoiceDto>>> GetSaleById(int id)
    {
        var result = await _salesService.GetSaleByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesInvoiceDto>>> CreateSale([FromBody] CreateSalesInvoiceDto dto)
    {
        var result = await _salesService.CreateSaleAsync(dto, null);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetSaleById), new { id = result.Data!.Id }, result);
    }
}

