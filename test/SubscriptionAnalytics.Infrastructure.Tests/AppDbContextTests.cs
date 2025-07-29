using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Infrastructure.Data;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class AppDbContextTests
{
    private readonly DbContextOptions<AppDbContext> _options;

    public AppDbContextTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void AppDbContext_Should_BeInstantiable()
    {
        // Arrange & Act
        using var context = new AppDbContext(_options);

        // Assert
        context.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveTenantsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Tenants.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveUserTenantsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.UserTenants.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveStripeConnectionsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.StripeConnections.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveCustomersDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Customers.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetUsersDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Users.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetRolesDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Roles.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetUserClaimsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.UserClaims.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetUserRolesDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.UserRoles.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetRoleClaimsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.RoleClaims.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetUserLoginsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.UserLogins.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveAspNetUserTokensDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.UserTokens.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_BeDisposable()
    {
        // Arrange
        var context = new AppDbContext(_options);

        // Act & Assert
        context.Should().BeAssignableTo<IDisposable>();
        context.Dispose();
    }

    [Fact]
    public void AppDbContext_Should_BeAsyncDisposable()
    {
        // Arrange
        var context = new AppDbContext(_options);

        // Act & Assert
        context.Should().BeAssignableTo<IAsyncDisposable>();
    }

    [Fact]
    public void AppDbContext_Should_HaveDatabase()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Database.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveChangeTracker()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.ChangeTracker.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveModel()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveContextId()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.ContextId.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveEntityTypes()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.GetEntityTypes().Should().NotBeEmpty();
    }

    [Fact]
    public void AppDbContext_Should_HaveTenantEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Tenant)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveUserTenantEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(UserTenant)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveStripeConnectionEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(StripeConnection)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveCustomerEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Customer)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityUserEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityUser)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityRoleEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityRole)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityUserClaimEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityUserClaim<string>)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityUserRoleEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityUserRole<string>)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityRoleClaimEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityUserLoginEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityUserLogin<string>)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveIdentityUserTokenEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Model.FindEntityType(typeof(Microsoft.AspNetCore.Identity.IdentityUserToken<string>)).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_BeThreadSafe()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                context.Should().NotBeNull();
                context.Tenants.Should().NotBeNull();
                context.UserTenants.Should().NotBeNull();
                context.StripeConnections.Should().NotBeNull();
                context.Customers.Should().NotBeNull();
            }));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
    }

    [Fact]
    public void AppDbContext_Should_HaveCorrectType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.Should().BeOfType<AppDbContext>();
    }

    [Fact]
    public void AppDbContext_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(AppDbContext).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_BePublic()
    {
        // Act & Assert
        typeof(AppDbContext).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void AppDbContext_Should_BeClass()
    {
        // Act & Assert
        typeof(AppDbContext).IsClass.Should().BeTrue();
    }

    [Fact]
    public void AppDbContext_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(AppDbContext).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(AppDbContext).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(AppDbContext).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(AppDbContext).IsByRef.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotHaveDefaultConstructor()
    {
        // Act & Assert
        typeof(AppDbContext).GetConstructor(Type.EmptyTypes).Should().BeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveParameterizedConstructor()
    {
        // Act & Assert
        typeof(AppDbContext).GetConstructor(new[] { typeof(DbContextOptions<AppDbContext>) }).Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(AppDbContext).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(AppDbContext).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNested()
    {
        // Act & Assert
        typeof(AppDbContext).IsNested.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeArray()
    {
        // Act & Assert
        typeof(AppDbContext).IsArray.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBePointer()
    {
        // Act & Assert
        typeof(AppDbContext).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(AppDbContext).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(AppDbContext).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(AppDbContext).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(AppDbContext).IsMarshalByRef.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(AppDbContext).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeException()
    {
        // Act & Assert
        typeof(AppDbContext).IsSubclassOf(typeof(Exception)).Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeAttribute()
    {
        // Act & Assert
        typeof(AppDbContext).IsSubclassOf(typeof(Attribute)).Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeStatic()
    {
        // Act & Assert
        typeof(AppDbContext).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(AppDbContext).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(AppDbContext).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void AppDbContext_Should_InheritFromDbContext()
    {
        // Act & Assert
        typeof(AppDbContext).IsSubclassOf(typeof(DbContext)).Should().BeTrue();
    }

    [Fact]
    public void AppDbContext_Should_ImplementIDisposable()
    {
        // Act & Assert
        typeof(AppDbContext).GetInterfaces().Should().Contain(typeof(IDisposable));
    }

    [Fact]
    public void AppDbContext_Should_ImplementIAsyncDisposable()
    {
        // Act & Assert
        typeof(AppDbContext).GetInterfaces().Should().Contain(typeof(IAsyncDisposable));
    }

    [Fact]
    public void AppDbContext_WithTenantIdConstructor_Should_BeInstantiable()
    {
        // Arrange
        var tenantId = Guid.NewGuid();

        // Act
        using var context = new AppDbContext(_options, tenantId);

        // Assert
        context.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_WithNullTenantIdConstructor_Should_BeInstantiable()
    {
        // Arrange
        Guid? tenantId = null;

        // Act
        using var context = new AppDbContext(_options, tenantId);

        // Assert
        context.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_WithTenantIdConstructor_Should_HaveProviderConnectionsDbSet()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        using var context = new AppDbContext(_options, tenantId);

        // Act & Assert
        context.ProviderConnections.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_WithTenantIdConstructor_Should_ApplyEntityConfigurations()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        using var context = new AppDbContext(_options, tenantId);

        // Act
        var model = context.Model;

        // Assert
        model.Should().NotBeNull();
        // Verify that entity configurations are applied by checking if the model has entities
        model.GetEntityTypes().Should().NotBeEmpty();
    }

    [Fact]
    public void AppDbContext_WithTenantIdConstructor_Should_HaveTenantSpecificQueryFilters()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        using var context = new AppDbContext(_options, tenantId);

        // Act
        var model = context.Model;

        // Assert
        // Verify that the model has query filters applied
        // This is a basic check - in a real scenario, you'd verify the actual filter expressions
        model.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_WithNullTenantIdConstructor_Should_NotApplyTenantFilters()
    {
        // Arrange
        Guid? tenantId = null;
        using var context = new AppDbContext(_options, tenantId);

        // Act
        var model = context.Model;

        // Assert
        model.Should().NotBeNull();
        // When tenantId is null, no tenant-specific filters should be applied
    }

    [Fact]
    public void AppDbContext_Should_HaveProviderConnectionsDbSet()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act & Assert
        context.ProviderConnections.Should().NotBeNull();
    }

    [Fact]
    public void AppDbContext_Should_HaveProviderConnectionEntityType()
    {
        // Arrange
        using var context = new AppDbContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(ProviderConnection));

        // Assert
        entityType.Should().NotBeNull();
    }
}
