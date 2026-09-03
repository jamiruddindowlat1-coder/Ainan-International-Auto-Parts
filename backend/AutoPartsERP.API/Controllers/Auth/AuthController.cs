using Microsoft.AspNetCore.Mvc;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;

namespace AutoPartsERP.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterUserDto request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetUsers()
    {
        var result = await _authService.GetAllUsersAsync();
        return Ok(result);
    }
}
