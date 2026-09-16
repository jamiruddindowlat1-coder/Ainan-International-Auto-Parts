using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Reports;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public ReportsController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<object>>> GetSummaryReport()
    {
        return Ok(await _accountingService.GetSummaryReportAsync());
    }
}
