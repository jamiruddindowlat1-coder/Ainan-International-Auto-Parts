using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Auth;

namespace AutoPartsERP.API.Controllers.Roles;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetRoles([FromQuery] string? search)
    {
        var query = _context.Roles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(r => r.Name.ToLower().Contains(s));
        }

        var list = await query
            .OrderBy(r => r.Name)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            })
            .ToListAsync();

        return Ok(ApiResponse<List<RoleDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleById(int id)
    {
        var r = await _context.Roles.FindAsync(id);
        if (r == null) return NotFound(ApiResponse<RoleDto>.Fail("Role not found"));

        return Ok(ApiResponse<RoleDto>.Ok(new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] RoleDto dto)
    {
        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        dto.Id = role.Id;
        return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, ApiResponse<RoleDto>.Ok(dto, "Role created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(int id, [FromBody] RoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound(ApiResponse<RoleDto>.Fail("Role not found"));

        role.Name = dto.Name;
        role.Description = dto.Description;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<RoleDto>.Ok(dto, "Role updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteRole(int id)
    {
        var role = await _context.Roles.Include(r => r.UserRoles).FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return NotFound(ApiResponse<bool>.Fail("Role not found"));

        if (role.UserRoles.Any())
        {
            return BadRequest(ApiResponse<bool>.Fail("Cannot delete role because it is assigned to users."));
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Role deleted successfully"));
    }
}

