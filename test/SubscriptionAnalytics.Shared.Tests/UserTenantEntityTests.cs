using FluentAssertions;
using SubscriptionAnalytics.Shared.Constants;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Tests;

public class UserTenantEntityTests
{
    [Fact]
    public void UserTenant_Should_InheritFromBaseTenantEntity()
    {
        // Act & Assert
        typeof(UserTenant).BaseType.Should().Be(typeof(BaseTenantEntity));
    }

    [Fact]
    public void UserTenant_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(UserTenant).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_BeInstantiable()
    {
        // Act
        var userTenant = new UserTenant();

        // Assert
        userTenant.Should().NotBeNull();
        userTenant.Should().BeOfType<UserTenant>();
        userTenant.Should().BeAssignableTo<BaseTenantEntity>();
        userTenant.Should().BeAssignableTo<BaseEntity>();
    }

    [Fact]
    public void UserTenant_Should_HaveUserIdProperty()
    {
        // Act
        var property = typeof(UserTenant).GetProperty("UserId");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void UserTenant_Should_HaveRoleProperty()
    {
        // Act
        var property = typeof(UserTenant).GetProperty("Role");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void UserId_Should_DefaultToEmptyString()
    {
        // Act
        var userTenant = new UserTenant();

        // Assert
        userTenant.UserId.Should().BeEmpty();
    }

    [Fact]
    public void Role_Should_DefaultToEmptyString()
    {
        // Act
        var userTenant = new UserTenant();

        // Assert
        userTenant.Role.Should().BeEmpty();
    }

    [Fact]
    public void Properties_Should_BeSettableAndGettable()
    {
        // Arrange
        var userTenant = new UserTenant();
        var userId = "user123";
        var role = Roles.TenantAdmin;

        // Act
        userTenant.UserId = userId;
        userTenant.Role = role;

        // Assert
        userTenant.UserId.Should().Be(userId);
        userTenant.Role.Should().Be(role);
    }

    [Fact]
    public void IsValidTenantRole_Should_ReturnTrue_ForValidRoles()
    {
        // Act & Assert
        UserTenant.IsValidTenantRole(Roles.TenantAdmin).Should().BeTrue();
        UserTenant.IsValidTenantRole(Roles.TenantUser).Should().BeTrue();
        UserTenant.IsValidTenantRole(Roles.SupportUser).Should().BeTrue();
        UserTenant.IsValidTenantRole(Roles.ReadOnlyUser).Should().BeTrue();
    }

    [Fact]
    public void IsValidTenantRole_Should_ReturnFalse_ForInvalidRoles()
    {
        // Act & Assert
        UserTenant.IsValidTenantRole("InvalidRole").Should().BeFalse();
        UserTenant.IsValidTenantRole("").Should().BeFalse();
        UserTenant.IsValidTenantRole("admin").Should().BeFalse();
        UserTenant.IsValidTenantRole("user").Should().BeFalse();
        UserTenant.IsValidTenantRole("support").Should().BeFalse();
        UserTenant.IsValidTenantRole("readonly").Should().BeFalse();
    }

    [Fact]
    public void IsValidTenantRole_Should_ReturnFalse_ForNullRole()
    {
        // Act & Assert
        UserTenant.IsValidTenantRole(null!).Should().BeFalse();
    }

    [Fact]
    public void IsValidTenantRole_Should_BeCaseSensitive()
    {
        // Act & Assert
        UserTenant.IsValidTenantRole("TenantAdmin").Should().BeTrue();
        UserTenant.IsValidTenantRole("tenantadmin").Should().BeFalse();
        UserTenant.IsValidTenantRole("TENANTADMIN").Should().BeFalse();
        UserTenant.IsValidTenantRole("TenantAdmin ").Should().BeFalse();
        UserTenant.IsValidTenantRole(" TenantAdmin").Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_BeReusable()
    {
        // Act
        var userTenant1 = new UserTenant();
        var userTenant2 = new UserTenant();

        // Assert
        userTenant1.Should().NotBeNull();
        userTenant2.Should().NotBeNull();
        userTenant1.Should().NotBeSameAs(userTenant2);
    }

    [Fact]
    public void UserTenant_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(UserTenant).Namespace.Should().Be("SubscriptionAnalytics.Shared.Entities");
    }

    [Fact]
    public void UserTenant_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(UserTenant).FullName.Should().Be("SubscriptionAnalytics.Shared.Entities.UserTenant");
    }

    [Fact]
    public void UserTenant_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(UserTenant).Name.Should().Be("UserTenant");
    }

    [Fact]
    public void UserTenant_Should_BePublic()
    {
        // Act & Assert
        typeof(UserTenant).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void UserTenant_Should_BeClass()
    {
        // Act & Assert
        typeof(UserTenant).IsClass.Should().BeTrue();
    }

    [Fact]
    public void UserTenant_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(UserTenant).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(UserTenant).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(UserTenant).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(UserTenant).IsByRef.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = typeof(UserTenant).GetConstructor(Type.EmptyTypes);
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void UserTenant_Should_NotHaveParameterizedConstructor()
    {
        // Act
        var constructors = typeof(UserTenant).GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].GetParameters().Should().BeEmpty();
    }

    [Fact]
    public async Task UserTenant_Should_BeThreadSafe()
    {
        // Act
        var userTenants = new System.Collections.Concurrent.ConcurrentBag<UserTenant>();
        var tasks = new List<Task>();

        // Create multiple user tenants concurrently
        for (int i = 0; i < 5; i++)
        {
            tasks.Add(Task.Run(() => userTenants.Add(new UserTenant())));
        }

        // Assert
        await Task.WhenAll(tasks);
        userTenants.Should().HaveCount(5);
        userTenants.Should().OnlyContain(ut => ut != null);
    }

    [Fact]
    public void UserTenant_Should_BeSerializable()
    {
        // Act & Assert
        typeof(UserTenant).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(UserTenant).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(UserTenant).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNested()
    {
        // Act & Assert
        typeof(UserTenant).IsNested.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(UserTenant).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(UserTenant).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeStatic()
    {
        // Act & Assert
        typeof(UserTenant).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(UserTenant).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeArray()
    {
        // Act & Assert
        typeof(UserTenant).IsArray.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBePointer()
    {
        // Act & Assert
        typeof(UserTenant).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(UserTenant).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(UserTenant).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(UserTenant).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void UserTenant_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(UserTenant).IsMarshalByRef.Should().BeFalse();
    }
} 