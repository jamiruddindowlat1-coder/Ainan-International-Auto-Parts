using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Auth;

namespace AutoPartsERP.API.Controllers.Users;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetUsers([FromQuery] string? search)
    {
        var query = _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower().Trim();
            query = query.Where(u => u.Username.ToLower().Contains(s) || u.Email.ToLower().Contains(s) || u.FullName.ToLower().Contains(s));
        }

        var list = await query
            .OrderBy(u => u.Username)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .ToListAsync();

        return Ok(ApiResponse<List<UserDto>>.Ok(list));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id)
    {
        var u = await _context.Users
            .Include(x => x.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
            
        if (u == null) return NotFound(ApiResponse<UserDto>.Fail("User not found"));

        return Ok(ApiResponse<UserDto>.Ok(new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            }));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] RegisterUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
        {
            return BadRequest(ApiResponse<UserDto>.Fail("Username or Email already exists"));
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone,
            PasswordHash = "AQAAAAEAACcQAAAAEJ8+3f6n4/KzQk/r6Q0tYgN8V5z7u0Z5gqV+Q9l4H8J1zV5m7Y1eT4W6g==", // Default hash like in AuthService
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var roleId in dto.RoleIds)
        {
            user.UserRoles.Add(new UserRole { RoleId = roleId });
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, ApiResponse<UserDto>.Ok(null, "User created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UserDto dto)
    {
        var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound(ApiResponse<UserDto>.Fail("User not found"));

        user.FullName = dto.FullName;
        user.Phone = dto.Phone;
        user.IsActive = dto.IsActive;
        // Not updating username/email for simplicity, or handle uniqueness check here.
        // Role update logic would go here (removing old roles, adding new).

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<UserDto>.Ok(dto, "User updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound(ApiResponse<bool>.Fail("User not found"));

        // Do not delete admin user
        if (user.Username.ToLower() == "admin")
        {
            return BadRequest(ApiResponse<bool>.Fail("Cannot delete the default admin user."));
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "User deleted successfully"));
    }
}

