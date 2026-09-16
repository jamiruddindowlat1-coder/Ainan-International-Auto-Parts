using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Income;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IncomeController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public IncomeController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<IncomeDto>>>> GetIncomes()
    {
        return Ok(await _accountingService.GetIncomesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<IncomeDto>>> CreateIncome([FromBody] IncomeDto dto)
    {
        return Ok(await _accountingService.CreateIncomeAsync(dto));
    }
}
