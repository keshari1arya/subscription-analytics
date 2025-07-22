using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Api.Controllers;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Shared.Constants;
using SubscriptionAnalytics.Shared.DTOs;

namespace SubscriptionAnalytics.Api.Tests;

public class TenantControllerTests
{
    private readonly Mock<ITenantService> _tenantServiceMock = new();
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock = MockUserManager();
    private readonly Mock<ILogger<TenantController>> _loggerMock = new();
    private readonly TenantController _controller;

    public TenantControllerTests()
    {
        _controller = new TenantController(_tenantServiceMock.Object, _userManagerMock.Object, _loggerMock.Object);
    }

    private static Mock<UserManager<IdentityUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task CreateTenant_Should_Return_CreatedAtAction()
    {
        // Arrange
        var request = new CreateTenantRequest { Name = "TestTenant" };
        var userId = "test-user-id";
        var user = new IdentityUser { Id = userId, Email = "test@example.com" };
        var tenantDto = new TenantDto { Id = Guid.NewGuid(), Name = request.Name, CreatedAt = DateTime.UtcNow, IsActive = true };
        
        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _tenantServiceMock.Setup(x => x.CreateTenantAsync(request, userId)).ReturnsAsync(tenantDto);

        // Act
        var result = await _controller.CreateTenant(request);

        // Assert
        var created = result.Result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created!.Value.Should().BeEquivalentTo(tenantDto);
    }

    [Fact]
    public async Task GetTenant_Should_Return_Ok_IfFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tenantDto = new TenantDto { Id = id, Name = "Tenant", CreatedAt = DateTime.UtcNow, IsActive = true };
        _tenantServiceMock.Setup(x => x.GetTenantByIdAsync(id)).ReturnsAsync(tenantDto);

        // Act
        var result = await _controller.GetTenant(id);

        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(tenantDto);
    }

    [Fact]
    public async Task GetTenant_Should_Return_NotFound_IfMissing()
    {
        // Arrange
        var id = Guid.NewGuid();
        _tenantServiceMock.Setup(x => x.GetTenantByIdAsync(id)).ReturnsAsync((TenantDto?)null);

        // Act
        var result = await _controller.GetTenant(id);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAllTenants_Should_Return_Ok()
    {
        // Arrange
        var tenants = new List<TenantDto> { new TenantDto { Id = Guid.NewGuid(), Name = "T1" } };
        _tenantServiceMock.Setup(x => x.GetAllTenantsAsync()).ReturnsAsync(tenants);

        // Act
        var result = await _controller.GetAllTenants();

        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(tenants);
    }

    [Fact]
    public async Task AssignUserToTenant_Should_Return_Ok()
    {
        // Arrange
        var request = new AssignUserToTenantRequest { UserEmail = "user@example.com", TenantId = Guid.NewGuid(), Role = Roles.TenantUser };
        var userTenantDto = new UserTenantDto { UserId = "user1", TenantId = request.TenantId, Role = request.Role, CreatedAt = DateTime.UtcNow };
        _tenantServiceMock.Setup(x => x.AssignUserToTenantAsync(request)).ReturnsAsync(userTenantDto);

        // Act
        var result = await _controller.AssignUserToTenant(request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedDto = okResult.Value.Should().BeOfType<UserTenantDto>().Subject;
        returnedDto.Should().BeEquivalentTo(userTenantDto);
    }

    [Fact]
    public async Task GetMyTenants_Should_Return_Ok_IfUserFound()
    {
        // Arrange
        var user = new IdentityUser { Id = "user1", Email = "user1@example.com" };
        var userTenants = new UserTenantsResponse { UserId = user.Id, UserEmail = user.Email!, Tenants = new List<UserTenantDto>() };
        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _tenantServiceMock.Setup(x => x.GetUserTenantsAsync(user.Id)).ReturnsAsync(userTenants);
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = await _controller.GetMyTenants();

        // Assert
        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.Value.Should().BeEquivalentTo(userTenants);
    }

    [Fact]
    public async Task GetMyTenants_Should_Return_Unauthorized_IfUserNotFound()
    {
        // Arrange
        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((IdentityUser?)null);
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = await _controller.GetMyTenants();

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task RemoveUserFromTenant_Should_Return_NoContent_IfSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var userId = "user1";
        _tenantServiceMock.Setup(x => x.RemoveUserFromTenantAsync(userId, tenantId)).ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveUserFromTenant(tenantId, userId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task RemoveUserFromTenant_Should_Return_NotFound_IfMissing()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var userId = "user1";
        _tenantServiceMock.Setup(x => x.RemoveUserFromTenantAsync(userId, tenantId)).ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveUserFromTenant(tenantId, userId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateUserTenantRole_Should_Return_NoContent_IfSuccess()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var userId = "user1";
        var newRole = Roles.TenantAdmin;
        _tenantServiceMock.Setup(x => x.UpdateUserTenantRoleAsync(userId, tenantId, newRole)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUserTenantRole(tenantId, userId, newRole);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateUserTenantRole_Should_Return_NotFound_IfFailed()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var userId = "user1";
        var newRole = Roles.TenantAdmin;
        _tenantServiceMock.Setup(x => x.UpdateUserTenantRoleAsync(userId, tenantId, newRole)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUserTenantRole(tenantId, userId, newRole);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task AssignAppRole_Should_Return_Ok_IfSuccess()
    {
        var request = new AssignAppRoleRequest { UserId = "user1", Role = Roles.AppAdmin };
        _tenantServiceMock.Setup(x => x.AssignAppRoleAsync(request.UserId, request.Role)).ReturnsAsync(true);
        var result = await _controller.AssignAppRole(request);
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task AssignAppRole_Should_Return_NotFound_IfUserMissing()
    {
        var request = new AssignAppRoleRequest { UserId = "user1", Role = Roles.AppAdmin };
        _tenantServiceMock.Setup(x => x.AssignAppRoleAsync(request.UserId, request.Role)).ReturnsAsync(false);
        var result = await _controller.AssignAppRole(request);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task RemoveAppRole_Should_Return_Ok_IfSuccess()
    {
        var request = new AssignAppRoleRequest { UserId = "user1", Role = Roles.AppAdmin };
        _tenantServiceMock.Setup(x => x.RemoveAppRoleAsync(request.UserId, request.Role)).ReturnsAsync(true);
        var result = await _controller.RemoveAppRole(request);
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task RemoveAppRole_Should_Return_NotFound_IfUserMissing()
    {
        var request = new AssignAppRoleRequest { UserId = "user1", Role = Roles.AppAdmin };
        _tenantServiceMock.Setup(x => x.RemoveAppRoleAsync(request.UserId, request.Role)).ReturnsAsync(false);
        var result = await _controller.RemoveAppRole(request);
        result.Should().BeOfType<NotFoundResult>();
    }
} 