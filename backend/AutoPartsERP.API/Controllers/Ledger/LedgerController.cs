using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Ledger;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LedgerController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public LedgerController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetLedger()
    {
        return Ok(await _accountingService.GetLedgerAsync());
    }
}
