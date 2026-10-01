using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Models.Auth;
using AutoPartsERP.API.Controllers.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsERP.Tests;

public class UsersControllerTests
{
    private static AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static RegisterUserDto NewDto(int roleId, string password = "Secret123!") => new()
    {
        Username = "testuser",
        Email = "test@example.com",
        Password = password,
        FullName = "Test User",
        RoleIds = new List<int> { roleId }
    };

    [Fact]
    public async Task CreateUser_StoresVerifiableHashOfGivenPassword()
    {
        using var context = NewContext();
        var role = new Role { Name = "Admin" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();
        var controller = new UsersController(context);

        await controller.CreateUser(NewDto(role.Id));

        var saved = context.Users.Single();
        var hasher = new PasswordHasher<User>();
        Assert.Equal(PasswordVerificationResult.Success, hasher.VerifyHashedPassword(saved, saved.PasswordHash, "Secret123!"));
        Assert.Equal(PasswordVerificationResult.Failed, hasher.VerifyHashedPassword(saved, saved.PasswordHash, "WrongPass"));
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUserWithIdAndRoleNames()
    {
        using var context = NewContext();
        var role = new Role { Name = "Admin" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();
        var controller = new UsersController(context);

        var result = await controller.CreateUser(NewDto(role.Id));

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var body = Assert.IsType<ApiResponse<UserDto>>(created.Value);
        Assert.NotNull(body.Data);
        Assert.True(body.Data!.Id > 0);
        Assert.Contains("Admin", body.Data.Roles);
    }

    [Fact]
    public async Task CreateUser_WithEmptyPassword_ReturnsBadRequestAndSavesNothing()
    {
        using var context = NewContext();
        var role = new Role { Name = "Admin" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();
        var controller = new UsersController(context);

        var result = await controller.CreateUser(NewDto(role.Id, ""));

        Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Empty(context.Users);
    }
}