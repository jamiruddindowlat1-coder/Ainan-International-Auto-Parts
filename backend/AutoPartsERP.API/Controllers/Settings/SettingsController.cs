using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Settings;

namespace AutoPartsERP.API.Controllers.Settings;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SettingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("company")]
    public async Task<ActionResult<ApiResponse<CompanySetting>>> GetCompanySettings()
    {
        var settings = await _context.CompanySettings.FirstOrDefaultAsync() ?? new CompanySetting();
        return Ok(ApiResponse<CompanySetting>.Ok(settings));
    }

    [HttpPut("company")]
    public async Task<ActionResult<ApiResponse<CompanySetting>>> UpdateCompanySettings([FromBody] CompanySetting updated)
    {
        var current = await _context.CompanySettings.FirstOrDefaultAsync();
        if (current == null)
        {
            await _context.CompanySettings.AddAsync(updated);
        }
        else
        {
            current.CompanyName = updated.CompanyName;
            current.ShortName = updated.ShortName;
            current.Tagline = updated.Tagline;
            current.Email = updated.Email;
            current.Phone = updated.Phone;
            current.Address = updated.Address;
            current.Website = updated.Website;
            current.CurrencyCode = updated.CurrencyCode;
            current.CurrencySymbol = updated.CurrencySymbol;
            current.FooterText = updated.FooterText;
            current.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<CompanySetting>.Ok(current ?? updated, "Company settings updated successfully"));
    }
}
