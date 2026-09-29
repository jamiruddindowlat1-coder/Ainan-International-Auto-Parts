using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Auth;

namespace AutoPartsERP.API.Controllers.Permissions;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PermissionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PermissionDto>>>> GetPermissions()
    {
        var list = await _context.Permissions.AsNoTracking()
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                ModuleName = p.ModuleName,
                ActionName = p.ActionName,
                Description = p.Description
            }).ToListAsync();
        return Ok(ApiResponse<List<PermissionDto>>.Ok(list));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PermissionDto>>> CreatePermission([FromBody] PermissionDto dto)
    {
        var permission = new Permission
        {
            ModuleName = dto.ModuleName,
            ActionName = dto.ActionName,
            Description = dto.Description
        };
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
        dto.Id = permission.Id;
        return Ok(ApiResponse<PermissionDto>.Ok(dto));
    }
}

