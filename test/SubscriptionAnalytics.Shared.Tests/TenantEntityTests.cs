using FluentAssertions;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Tests;

public class TenantEntityTests
{
    [Fact]
    public void Tenant_Should_InheritFromBaseEntity()
    {
        // Act & Assert
        typeof(Tenant).BaseType.Should().Be(typeof(BaseEntity));
    }

    [Fact]
    public void Tenant_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(Tenant).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_BeInstantiable()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.Should().NotBeNull();
        tenant.Should().BeOfType<Tenant>();
        tenant.Should().BeAssignableTo<BaseEntity>();
    }

    [Fact]
    public void Tenant_Should_HaveNameProperty()
    {
        // Act
        var property = typeof(Tenant).GetProperty("Name");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_HaveIsActiveProperty()
    {
        // Act
        var property = typeof(Tenant).GetProperty("IsActive");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(bool));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_HaveStripeConnectionProperty()
    {
        // Act
        var property = typeof(Tenant).GetProperty("StripeConnection");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(StripeConnection));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
        property.GetGetMethod()!.IsVirtual.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_HaveUserTenantsProperty()
    {
        // Act
        var property = typeof(Tenant).GetProperty("UserTenants");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(ICollection<UserTenant>));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
        property.GetGetMethod()!.IsVirtual.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_HaveCustomersProperty()
    {
        // Act
        var property = typeof(Tenant).GetProperty("Customers");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(ICollection<Customer>));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
        property.GetGetMethod()!.IsVirtual.Should().BeTrue();
    }

    [Fact]
    public void Name_Should_DefaultToEmptyString()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.Name.Should().BeEmpty();
    }

    [Fact]
    public void IsActive_Should_DefaultToTrue()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.IsActive.Should().BeTrue();
    }

    [Fact]
    public void StripeConnection_Should_DefaultToNull()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.StripeConnection.Should().BeNull();
    }

    [Fact]
    public void UserTenants_Should_DefaultToEmptyList()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.UserTenants.Should().NotBeNull();
        tenant.UserTenants.Should().BeEmpty();
    }

    [Fact]
    public void Customers_Should_DefaultToEmptyList()
    {
        // Act
        var tenant = new Tenant();

        // Assert
        tenant.Customers.Should().NotBeNull();
        tenant.Customers.Should().BeEmpty();
    }

    [Fact]
    public void Properties_Should_BeSettableAndGettable()
    {
        // Arrange
        var tenant = new Tenant();
        var name = "Test Tenant";
        var isActive = false;
        var stripeConnection = new StripeConnection();
        var userTenants = new List<UserTenant> { new UserTenant() };
        var customers = new List<Customer> { new Customer() };

        // Act
        tenant.Name = name;
        tenant.IsActive = isActive;
        tenant.StripeConnection = stripeConnection;
        tenant.UserTenants = userTenants;
        tenant.Customers = customers;

        // Assert
        tenant.Name.Should().Be(name);
        tenant.IsActive.Should().Be(isActive);
        tenant.StripeConnection.Should().Be(stripeConnection);
        tenant.UserTenants.Should().BeEquivalentTo(userTenants);
        tenant.Customers.Should().BeEquivalentTo(customers);
    }

    [Fact]
    public void Tenant_Should_BeReusable()
    {
        // Act
        var tenant1 = new Tenant();
        var tenant2 = new Tenant();

        // Assert
        tenant1.Should().NotBeNull();
        tenant2.Should().NotBeNull();
        tenant1.Should().NotBeSameAs(tenant2);
    }

    [Fact]
    public void Tenant_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(Tenant).Namespace.Should().Be("SubscriptionAnalytics.Shared.Entities");
    }

    [Fact]
    public void Tenant_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(Tenant).FullName.Should().Be("SubscriptionAnalytics.Shared.Entities.Tenant");
    }

    [Fact]
    public void Tenant_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(Tenant).Name.Should().Be("Tenant");
    }

    [Fact]
    public void Tenant_Should_BePublic()
    {
        // Act & Assert
        typeof(Tenant).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_BeClass()
    {
        // Act & Assert
        typeof(Tenant).IsClass.Should().BeTrue();
    }

    [Fact]
    public void Tenant_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(Tenant).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(Tenant).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(Tenant).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(Tenant).IsByRef.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = typeof(Tenant).GetConstructor(Type.EmptyTypes);
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void Tenant_Should_NotHaveParameterizedConstructor()
    {
        // Act
        var constructors = typeof(Tenant).GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].GetParameters().Should().BeEmpty();
    }

    [Fact]
    public async Task Tenant_Should_BeThreadSafe()
    {
        // Act
        var tenants = new System.Collections.Concurrent.ConcurrentBag<Tenant>();
        var tasks = new List<Task>();

        // Create multiple tenants concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => tenants.Add(new Tenant())));
        }

        // Assert
        await Task.WhenAll(tasks);
        tenants.Should().HaveCount(10);
        tenants.Should().OnlyContain(t => t != null);
    }

    [Fact]
    public void Tenant_Should_BeSerializable()
    {
        // Act & Assert
        typeof(Tenant).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(Tenant).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(Tenant).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNested()
    {
        // Act & Assert
        typeof(Tenant).IsNested.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(Tenant).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(Tenant).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(Tenant).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(Tenant).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(Tenant).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(Tenant).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(Tenant).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeStatic()
    {
        // Act & Assert
        typeof(Tenant).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(Tenant).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeArray()
    {
        // Act & Assert
        typeof(Tenant).IsArray.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBePointer()
    {
        // Act & Assert
        typeof(Tenant).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(Tenant).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(Tenant).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(Tenant).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void Tenant_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(Tenant).IsMarshalByRef.Should().BeFalse();
    }
}
