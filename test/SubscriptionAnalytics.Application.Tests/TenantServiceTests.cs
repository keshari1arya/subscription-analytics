using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Application.Services;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Exceptions;
using Xunit;

namespace SubscriptionAnalytics.Application.Tests;

public class TenantServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<ILogger<TenantService>> _loggerMock;
    private readonly TenantService _service;

    public TenantServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);
        _userManagerMock = MockUserManager();
        _loggerMock = new Mock<ILogger<TenantService>>();
        _service = new TenantService(_dbContext, _userManagerMock.Object, _loggerMock.Object);
    }

    private static Mock<UserManager<IdentityUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task CreateTenantAsync_Should_Create_And_Return_TenantDto()
    {
        // Arrange
        var request = new CreateTenantRequest { Name = "TestTenant" };
        _dbContext.Tenants.Add(new Tenant { Name = request.Name, IsActive = true });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.CreateTenantAsync(request);

        // Assert
        result.Name.Should().Be("TestTenant");
        result.Id.Should().NotBeEmpty();
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetTenantByIdAsync_Should_Return_TenantDto_If_Found()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant { Id = tenantId, Name = "Tenant1", CreatedAt = DateTime.UtcNow, IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetTenantByIdAsync(tenantId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(tenantId);
        result.Name.Should().Be("Tenant1");
    }

    [Fact]
    public async Task GetTenantByIdAsync_Should_Return_Null_If_NotFound()
    {
        // Arrange
        var result = await _service.GetTenantByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllTenantsAsync_Should_Return_ActiveTenants_OrderedByName()
    {
        // Arrange
        _dbContext.Tenants.AddRange(
            new Tenant { Id = Guid.NewGuid(), Name = "B", IsActive = true },
            new Tenant { Id = Guid.NewGuid(), Name = "A", IsActive = true },
            new Tenant { Id = Guid.NewGuid(), Name = "C", IsActive = false }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAllTenantsAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("A");
        result[1].Name.Should().Be("B");
    }

    [Fact]
    public async Task GetAllTenantsAsync_Should_Return_EmptyList_IfNone()
    {
        // Act
        var result = await _service.GetAllTenantsAsync();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AssignUserToTenantAsync_Should_Throw_IfUserNotFound()
    {
        // Arrange
        var request = new AssignUserToTenantRequest { UserEmail = "nouser@example.com", TenantId = Guid.NewGuid(), Role = "Admin" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.UserEmail)).ReturnsAsync((IdentityUser?)null);

        // Act
        var act = async () => await _service.AssignUserToTenantAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*nouser@example.com*");
    }

    [Fact]
    public async Task AssignUserToTenantAsync_Should_Throw_IfTenantNotFound()
    {
        // Arrange
        var user = new IdentityUser { Id = "user1", Email = "user1@example.com" };
        var request = new AssignUserToTenantRequest { UserEmail = user.Email!, TenantId = Guid.NewGuid(), Role = "Admin" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        // No tenant in db

        // Act
        var act = async () => await _service.AssignUserToTenantAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*Tenant*");
    }

    [Fact]
    public async Task AssignUserToTenantAsync_Should_AssignUser_IfNotAssigned()
    {
        // Arrange
        var user = new IdentityUser { Id = "user2", Email = "user2@example.com" };
        var tenant = new Tenant { Id = Guid.NewGuid(), Name = "TenantX", IsActive = true };
        _dbContext.Tenants.Add(tenant);
        await _dbContext.SaveChangesAsync();
        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        var request = new AssignUserToTenantRequest { UserEmail = user.Email!, TenantId = tenant.Id, Role = "User" };

        // Act
        var result = await _service.AssignUserToTenantAsync(request);

        // Assert
        result.UserId.Should().Be(user.Id);
        result.TenantId.Should().Be(tenant.Id);
        result.Role.Should().Be("User");
        result.TenantName.Should().Be(tenant.Name);
        result.UserEmail.Should().Be(user.Email);
    }

    [Fact]
    public async Task AssignUserToTenantAsync_Should_UpdateRole_IfAlreadyAssigned()
    {
        // Arrange
        var user = new IdentityUser { Id = "user3", Email = "user3@example.com" };
        var tenant = new Tenant { Id = Guid.NewGuid(), Name = "TenantY", IsActive = true };
        var userTenant = new UserTenant { UserId = user.Id, TenantId = tenant.Id, Role = "OldRole", CreatedAt = DateTime.UtcNow };
        _dbContext.Tenants.Add(tenant);
        _dbContext.UserTenants.Add(userTenant);
        await _dbContext.SaveChangesAsync();
        _userManagerMock.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        var request = new AssignUserToTenantRequest { UserEmail = user.Email!, TenantId = tenant.Id, Role = "NewRole" };

        // Act
        var result = await _service.AssignUserToTenantAsync(request);

        // Assert
        result.Role.Should().Be("NewRole");
    }

    [Fact]
    public async Task GetUserTenantsAsync_Should_Throw_IfUserNotFound()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync("badid")).ReturnsAsync((IdentityUser?)null);

        // Act
        var act = async () => await _service.GetUserTenantsAsync("badid");

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*badid*");
    }

    [Fact]
    public async Task GetUserTenantsAsync_Should_Return_TenantsForUser()
    {
        // Arrange
        var user = new IdentityUser { Id = "user4", Email = "user4@example.com" };
        var tenant = new Tenant { Id = Guid.NewGuid(), Name = "TenantZ", IsActive = true };
        var userTenant = new UserTenant { UserId = user.Id, TenantId = tenant.Id, Role = "Role", CreatedAt = DateTime.UtcNow, Tenant = tenant };
        _dbContext.Tenants.Add(tenant);
        _dbContext.UserTenants.Add(userTenant);
        await _dbContext.SaveChangesAsync();
        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _service.GetUserTenantsAsync(user.Id);

        // Assert
        result.UserId.Should().Be(user.Id);
        result.UserEmail.Should().Be(user.Email);
        result.Tenants.Should().HaveCount(1);
        result.Tenants[0].TenantId.Should().Be(tenant.Id);
    }

    [Fact]
    public async Task RemoveUserFromTenantAsync_Should_ReturnFalse_IfNotFound()
    {
        // Act
        var result = await _service.RemoveUserFromTenantAsync("nouser", Guid.NewGuid());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveUserFromTenantAsync_Should_Remove_And_ReturnTrue()
    {
        // Arrange
        var userTenant = new UserTenant { UserId = "user5", TenantId = Guid.NewGuid(), Role = "Role", CreatedAt = DateTime.UtcNow };
        _dbContext.UserTenants.Add(userTenant);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.RemoveUserFromTenantAsync(userTenant.UserId, userTenant.TenantId);

        // Assert
        result.Should().BeTrue();
        _dbContext.UserTenants.Find(userTenant.UserId, userTenant.TenantId).Should().BeNull();
    }

    [Fact]
    public async Task UpdateUserTenantRoleAsync_Should_ReturnFalse_IfNotFound()
    {
        // Act
        var result = await _service.UpdateUserTenantRoleAsync("nouser", Guid.NewGuid(), "Role");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateUserTenantRoleAsync_Should_UpdateRole_And_ReturnTrue()
    {
        // Arrange
        var userTenant = new UserTenant { UserId = "user6", TenantId = Guid.NewGuid(), Role = "OldRole", CreatedAt = DateTime.UtcNow };
        _dbContext.UserTenants.Add(userTenant);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.UpdateUserTenantRoleAsync(userTenant.UserId, userTenant.TenantId, "NewRole");

        // Assert
        result.Should().BeTrue();
        _dbContext.UserTenants.Find(userTenant.UserId, userTenant.TenantId)?.Role.Should().Be("NewRole");
    }
} 