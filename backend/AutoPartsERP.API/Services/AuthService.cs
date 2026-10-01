using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Auth;

namespace AutoPartsERP.API.Services;

public class AuthService : IAuthService
{

    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower() || u.Email.ToLower() == request.Username.ToLower());

        if (user == null || !user.IsActive)
        {
            return ApiResponse<LoginResponseDto>.Fail("Invalid username or password");
        }

        // Password check using ASP.NET Core Identity PasswordHasher
        bool isValid;
        try
        {
            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            isValid = verify != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            // Stored hash is not a valid Identity hash (e.g. Google-only account)
            isValid = false;
        }

        if (!isValid)
        {
            return ApiResponse<LoginResponseDto>.Fail("Invalid username or password");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var roles = user.UserRoles.Select(r => r.Role.Name).ToList();
        var token = GenerateJwtToken(user, roles);

        return ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        }, "Login successful");
    }

    public async Task<ApiResponse<LoginResponseDto>> GoogleLoginAsync(GoogleLoginRequestDto request)
    {
        var clientId = _config["Google:ClientId"];
        if (string.IsNullOrWhiteSpace(clientId))
            return ApiResponse<LoginResponseDto>.Fail("Google login is not configured");

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = new[] { clientId } });
        }
        catch (InvalidJwtException)
        {
            return ApiResponse<LoginResponseDto>.Fail("Invalid Google token");
        }

        if (!payload.EmailVerified)
            return ApiResponse<LoginResponseDto>.Fail("Google email is not verified");

        var email = payload.Email.ToLower();

        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject || u.Email.ToLower() == email);

        if (user == null)
        {
            // New user: created inactive, an administrator must approve it
            user = new User
            {
                Username = email,
                Email = payload.Email,
                FullName = payload.Name ?? payload.Email,
                PasswordHash = Guid.NewGuid().ToString("N"),
                GoogleId = payload.Subject,
                PictureUrl = payload.Picture,
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return ApiResponse<LoginResponseDto>.Fail("Account created. Waiting for administrator approval.");
        }

        if (!string.IsNullOrEmpty(user.GoogleId) && user.GoogleId != payload.Subject)
            return ApiResponse<LoginResponseDto>.Fail("This account is linked to a different Google account");

        if (!user.IsActive)
            return ApiResponse<LoginResponseDto>.Fail("Account is inactive");

        user.GoogleId = payload.Subject;
        user.PictureUrl = payload.Picture;
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var roles = user.UserRoles.Select(r => r.Role.Name).ToList();
        var token = GenerateJwtToken(user, roles);

        return ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        }, "Login successful");
    }

    public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterUserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower()))
        {
            return ApiResponse<UserDto>.Fail("Username is already taken");
        }

        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
        {
            return ApiResponse<UserDto>.Fail("Email is already registered");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            Phone = request.Phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Real password hash (replaces the old fixed placeholder hash)
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var roleNames = new List<string>();
        if (request.RoleIds.Any())
        {
            foreach (var roleId in request.RoleIds)
            {
                await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = roleId });
            }
            await _context.SaveChangesAsync();

            roleNames = await _context.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();
        }

        return ApiResponse<UserDto>.Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Phone = user.Phone,
            IsActive = user.IsActive,
            Roles = roleNames,
            CreatedAt = user.CreatedAt
        }, "User registered successfully");
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                Phone = u.Phone,
                IsActive = u.IsActive,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList(),
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return ApiResponse<List<UserDto>>.Ok(users);
    }

    private string GenerateJwtToken(User user, List<string> roles)
    {
        // Empty string in config must also fall back (?? only handles null)
        var configuredSecret = _config["JwtSettings:Secret"];
        var secret = string.IsNullOrWhiteSpace(configuredSecret) ? throw new InvalidOperationException("JwtSettings:Secret is not configured.") : configuredSecret;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new("FullName", user.FullName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"] ?? "AIAPS_API",
            audience: _config["JwtSettings:Audience"] ?? "AIAPS_Client",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}