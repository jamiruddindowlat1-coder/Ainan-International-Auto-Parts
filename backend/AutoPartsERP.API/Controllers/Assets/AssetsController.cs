using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Assets;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IAccountingService _accountingService;

    public AssetsController(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AssetDto>>>> GetAssets()
    {
        return Ok(await _accountingService.GetAssetsAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AssetDto>>> CreateAsset([FromBody] AssetDto dto)
    {
        return Ok(await _accountingService.CreateAssetAsync(dto));
    }
}
