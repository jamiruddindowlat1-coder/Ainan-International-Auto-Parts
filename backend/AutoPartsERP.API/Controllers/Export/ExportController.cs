using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Export;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly IPartsService _partsService;
    private readonly ISalesService _salesService;
    private readonly IExportService _exportService;

    public ExportController(IPartsService partsService, ISalesService salesService, IExportService exportService)
    {
        _partsService = partsService;
        _salesService = salesService;
        _exportService = exportService;
    }

    [HttpGet("parts/excel")]
    public async Task<IActionResult> ExportPartsExcel()
    {
        var response = await _partsService.GetPartsAsync(null, null, null, 1, 5000);
        var bytes = _exportService.ExportPartsToExcel(response.Data);
        var filename = $"AIAPS_Parts_Catalog_{DateTime.UtcNow:yyyyMMdd}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    }

    [HttpGet("sales/excel")]
    public async Task<IActionResult> ExportSalesExcel()
    {
        var response = await _salesService.GetSalesAsync(null, null, null, null, 1, 5000);
        var bytes = _exportService.ExportSalesToExcel(response.Data);
        var filename = $"AIAPS_Sales_Report_{DateTime.UtcNow:yyyyMMdd}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
    }
}

