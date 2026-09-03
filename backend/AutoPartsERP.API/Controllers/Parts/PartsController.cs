using Microsoft.AspNetCore.Mvc;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Parts;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly IPartsService _partsService;

    public PartsController(IPartsService partsService)
    {
        _partsService = partsService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<PartDto>>> GetParts(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] int? brandId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _partsService.GetPartsAsync(search, categoryId, brandId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PartDto>>> GetPartById(int id)
    {
        var result = await _partsService.GetPartByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PartDto>>> CreatePart([FromBody] CreatePartDto dto)
    {
        var result = await _partsService.CreatePartAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetPartById), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<PartDto>>> UpdatePart(int id, [FromBody] CreatePartDto dto)
    {
        var result = await _partsService.UpdatePartAsync(id, dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeletePart(int id)
    {
        var result = await _partsService.DeletePartAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetCategories()
    {
        return Ok(await _partsService.GetCategoriesAsync());
    }

    [HttpGet("brands")]
    public async Task<ActionResult<ApiResponse<List<BrandDto>>>> GetBrands()
    {
        return Ok(await _partsService.GetBrandsAsync());
    }

    [HttpGet("units")]
    public async Task<ActionResult<ApiResponse<List<UnitDto>>>> GetUnits()
    {
        return Ok(await _partsService.GetUnitsAsync());
    }
}
