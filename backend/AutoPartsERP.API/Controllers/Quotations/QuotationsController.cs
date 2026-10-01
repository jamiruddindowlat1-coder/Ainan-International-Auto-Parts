using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Quotations;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuotationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IQuotationService _service;

    public QuotationsController(AppDbContext context, IQuotationService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<QuotationDto>>>> GetQuotations()
    {
        var list = await _context.Quotations.AsNoTracking()
            .Select(q => new QuotationDto
            {
                Id = q.Id,
                QuotationNumber = q.QuotationNumber,
                CustomerId = q.CustomerId,
                TotalAmount = q.TotalAmount,
                QuotationDate = q.CreatedAt
            }).ToListAsync();
        return Ok(ApiResponse<List<QuotationDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<QuotationResultDto>>> CreateQuotation([FromBody] CreateQuotationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}