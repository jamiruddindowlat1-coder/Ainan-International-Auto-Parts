using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Expenses;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public ExpensesController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ExpenseDto>>>> GetExpenses()
    {
        return Ok(await _accountingService.GetExpensesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ExpenseDto>>> CreateExpense([FromBody] ExpenseDto dto)
    {
        return Ok(await _accountingService.CreateExpenseAsync(dto));
    }
}
