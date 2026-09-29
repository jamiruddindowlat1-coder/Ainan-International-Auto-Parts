using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Liabilities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LiabilitiesController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public LiabilitiesController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LiabilityDto>>>> GetLiabilities()
    {
        return Ok(await _accountingService.GetLiabilitiesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LiabilityDto>>> CreateLiability([FromBody] LiabilityDto dto)
    {
        return Ok(await _accountingService.CreateLiabilityAsync(dto));
    }
}
