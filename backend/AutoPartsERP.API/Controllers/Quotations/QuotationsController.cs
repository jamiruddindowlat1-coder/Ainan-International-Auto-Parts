using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Sales;

namespace AutoPartsERP.API.Controllers.Quotations;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class QuotationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuotationsController(AppDbContext context)
    {
        _context = context;
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
    public async Task<ActionResult<ApiResponse<QuotationDto>>> CreateQuotation([FromBody] QuotationDto dto)
    {
        var quotation = new Quotation
        {
            QuotationNumber = string.IsNullOrWhiteSpace(dto.QuotationNumber) ? "QT-" + DateTime.Now.Ticks : dto.QuotationNumber,
            CustomerId = dto.CustomerId,
            TotalAmount = dto.TotalAmount,
            CreatedAt = dto.QuotationDate == default ? DateTime.UtcNow : dto.QuotationDate,
            ExpiryDate = DateTime.UtcNow.AddDays(30)
        };
        await _context.Quotations.AddAsync(quotation);
        await _context.SaveChangesAsync();
        dto.Id = quotation.Id;
        return Ok(ApiResponse<QuotationDto>.Ok(dto));
    }
}

