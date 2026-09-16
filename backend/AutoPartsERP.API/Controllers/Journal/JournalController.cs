using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Journal;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class JournalController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public JournalController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<JournalEntryDto>>>> GetJournalEntries()
    {
        return Ok(await _accountingService.GetJournalEntriesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> CreateJournalEntry([FromBody] JournalEntryDto dto)
    {
        return Ok(await _accountingService.CreateJournalEntryAsync(dto));
    }
}
