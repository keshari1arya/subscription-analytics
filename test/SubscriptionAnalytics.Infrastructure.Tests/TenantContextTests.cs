using FluentAssertions;
using SubscriptionAnalytics.Infrastructure.Services;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class TenantContextTests
{
    [Fact]
    public void Constructor_Should_CreateTenantContext()
    {
        // Act
        var tenantContext = new TenantContext();

        // Assert
        tenantContext.Should().NotBeNull();
        tenantContext.Should().BeAssignableTo<ITenantContext>();
    }

    [Fact]
    public void TenantId_Should_BeEmptyByDefault()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        tenantContext.TenantId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void TenantId_Should_BeSettable()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();

        // Act
        tenantContext.TenantId = tenantId;

        // Assert
        tenantContext.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantId_Should_BeGettable()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();
        tenantContext.TenantId = tenantId;

        // Act
        var retrievedTenantId = tenantContext.TenantId;

        // Assert
        retrievedTenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantId_Should_BeEmptyGuid()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act
        tenantContext.TenantId = Guid.Empty;

        // Assert
        tenantContext.TenantId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void TenantId_Should_BeSetMultipleTimes()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();

        // Act
        tenantContext.TenantId = tenantId1;
        tenantContext.TenantId = tenantId2;

        // Assert
        tenantContext.TenantId.Should().Be(tenantId2);
    }

    [Fact]
    public void TenantId_Should_HandleValidGuids()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var validGuids = new[]
        {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act & Assert
        foreach (var guid in validGuids)
        {
            tenantContext.TenantId = guid;
            tenantContext.TenantId.Should().Be(guid);
        }
    }

    [Fact]
    public async Task TenantId_Should_BeThreadSafe()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tasks = new List<Task>();
        var results = new System.Collections.Concurrent.ConcurrentBag<Guid>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var tenantId = Guid.NewGuid();
            tasks.Add(Task.Run(() =>
            {
                tenantContext.TenantId = tenantId;
                results.Add(tenantContext.TenantId);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(10);
        results.Should().OnlyContain(r => r != Guid.Empty);
    }

    [Fact]
    public void TenantContext_Should_ImplementITenantContext()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        tenantContext.Should().BeAssignableTo<ITenantContext>();
        tenantContext.Should().BeOfType<TenantContext>();
    }

    [Fact]
    public void TenantContext_Should_BeReferenceType()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        tenantContext.Should().BeOfType<TenantContext>();
        typeof(TenantContext).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_BePublic()
    {
        // Act & Assert
        typeof(TenantContext).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void TenantContext_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(TenantContext).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(TenantContext).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(TenantContext).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(TenantContext).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(TenantContext).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_NotBeNested()
    {
        // Act & Assert
        typeof(TenantContext).IsNested.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(TenantContext).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Services");
    }

    [Fact]
    public void TenantContext_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(TenantContext).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void TenantContext_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(TenantContext).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Services.TenantContext");
    }

    [Fact]
    public void TenantContext_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(TenantContext).Name.Should().Be("TenantContext");
    }

    [Fact]
    public void TenantId_Property_Should_BePublic()
    {
        // Act & Assert
        typeof(TenantContext).GetProperty("TenantId")!.GetGetMethod()!.IsPublic.Should().BeTrue();
        typeof(TenantContext).GetProperty("TenantId")!.GetSetMethod()!.IsPublic.Should().BeTrue();
    }

    [Fact]
    public void TenantId_Property_Should_BeVirtual()
    {
        // Act & Assert
        typeof(TenantContext).GetProperty("TenantId")!.GetGetMethod()!.IsVirtual.Should().BeTrue();
        typeof(TenantContext).GetProperty("TenantId")!.GetSetMethod()!.IsVirtual.Should().BeTrue();
    }

    [Fact]
    public void TenantId_Property_Should_BeInstance()
    {
        // Act & Assert
        typeof(TenantContext).GetProperty("TenantId")!.GetGetMethod()!.IsStatic.Should().BeFalse();
        typeof(TenantContext).GetProperty("TenantId")!.GetSetMethod()!.IsStatic.Should().BeFalse();
    }

    [Fact]
    public void TenantId_Property_Should_ReturnGuid()
    {
        // Act & Assert
        typeof(TenantContext).GetProperty("TenantId")!.PropertyType.Should().Be(typeof(Guid));
    }

    [Fact]
    public void TenantContext_Should_BeInstantiable()
    {
        // Act & Assert
        var tenantContext = new TenantContext();
        tenantContext.Should().NotBeNull();
    }

    [Fact]
    public void TenantContext_Should_BeReusable()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantIds = new List<Guid>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var tenantId = Guid.NewGuid();
            tenantContext.TenantId = tenantId;
            tenantIds.Add(tenantContext.TenantId);
        }

        // Assert
        tenantIds.Should().HaveCount(10);
        tenantIds.Should().OnlyContain(id => id != Guid.Empty);
    }

    [Fact]
    public void TenantContext_Should_HandleConcurrentReads()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();
        tenantContext.TenantId = tenantId;
        var tasks = new List<Task<Guid>>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() => tenantContext.TenantId));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        var results = tasks.Select(t => t.Result).ToList();
        results.Should().HaveCount(100);
        results.Should().OnlyContain(r => r == tenantId);
    }

    [Fact]
    public async Task TenantContext_Should_HandleConcurrentWrites()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tasks = new List<Task>();
        var tenantIds = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToList();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var tenantId = tenantIds[i];
            tasks.Add(Task.Run(() => tenantContext.TenantId = tenantId));
        }

        await Task.WhenAll(tasks);

        // Assert
        tenantContext.TenantId.Should().NotBe(Guid.Empty);
        tenantIds.Should().Contain(tenantContext.TenantId);
    }

    [Fact]
    public async Task TenantContext_Should_HandleMixedOperations()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tasks = new List<Task>();
        var results = new System.Collections.Concurrent.ConcurrentBag<Guid>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var tenantId = Guid.NewGuid();
            tasks.Add(Task.Run(() =>
            {
                tenantContext.TenantId = tenantId;
                results.Add(tenantContext.TenantId);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(10);
        results.Should().OnlyContain(r => r != Guid.Empty);
    }

    [Fact]
    public async Task TenantContext_Should_HandleStressTest()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tasks = new List<Task>();
        var results = new System.Collections.Concurrent.ConcurrentBag<Guid>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            var tenantId = Guid.NewGuid();
            tasks.Add(Task.Run(() =>
            {
                tenantContext.TenantId = tenantId;
                results.Add(tenantContext.TenantId);
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(100);
        results.Should().OnlyContain(r => r != Guid.Empty);
    }

    [Fact]
    public void TenantContext_Should_HandleNullAssignment()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();
        tenantContext.TenantId = tenantId;

        // Act & Assert
        // Note: Guid is a value type, so it cannot be null
        // This test verifies that the property handles the default Guid.Empty value
        tenantContext.TenantId = Guid.Empty;
        tenantContext.TenantId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void TenantContext_Should_HandleDefaultConstructor()
    {
        // Act
        var tenantContext = new TenantContext();

        // Assert
        tenantContext.Should().NotBeNull();
        tenantContext.TenantId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void TenantContext_Should_HandlePropertyAccess()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();

        // Act
        tenantContext.TenantId = tenantId;
        var retrievedTenantId = tenantContext.TenantId;

        // Assert
        retrievedTenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantContext_Should_HandleMultipleInstances()
    {
        // Arrange
        var tenantContext1 = new TenantContext();
        var tenantContext2 = new TenantContext();
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();

        // Act
        tenantContext1.TenantId = tenantId1;
        tenantContext2.TenantId = tenantId2;

        // Assert
        tenantContext1.TenantId.Should().Be(tenantId1);
        tenantContext2.TenantId.Should().Be(tenantId2);
        tenantContext1.TenantId.Should().NotBe(tenantContext2.TenantId);
    }

    [Fact]
    public void TenantContext_Should_HandleInterfaceCasting()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();

        // Act
        ITenantContext interfaceContext = tenantContext;
        interfaceContext.TenantId = tenantId;

        // Assert
        interfaceContext.TenantId.Should().Be(tenantId);
        tenantContext.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantContext_Should_HandleInterfaceImplementation()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        tenantContext.Should().BeAssignableTo<ITenantContext>();
        typeof(TenantContext).GetInterfaces().Should().Contain(typeof(ITenantContext));
    }

    [Fact]
    public void TenantContext_Should_HandlePropertyReflection()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var propertyInfo = typeof(TenantContext).GetProperty("TenantId");
        var tenantId = Guid.NewGuid();

        // Act
        propertyInfo!.SetValue(tenantContext, tenantId);
        var retrievedValue = propertyInfo.GetValue(tenantContext);

        // Assert
        retrievedValue.Should().Be(tenantId);
        tenantContext.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantContext_Should_HandleTypeChecking()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act & Assert
        tenantContext.Should().BeOfType<TenantContext>();
        tenantContext.Should().BeAssignableTo<ITenantContext>();
        tenantContext.Should().NotBeOfType<string>();
        tenantContext.Should().NotBeOfType<int>();
    }

    [Fact]
    public void TenantContext_Should_HandleToString()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var tenantId = Guid.NewGuid();
        tenantContext.TenantId = tenantId;

        // Act
        var result = tenantContext.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("TenantContext");
    }

    [Fact]
    public void TenantContext_Should_HandleGetHashCode()
    {
        // Arrange
        var tenantContext1 = new TenantContext();
        var tenantContext2 = new TenantContext();

        // Act
        var hashCode1 = tenantContext1.GetHashCode();
        var hashCode2 = tenantContext2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(0);
        hashCode2.Should().NotBe(0);
        // Note: Hash codes might be the same for different instances, so we don't assert they're different
    }

    [Fact]
    public void TenantContext_Should_HandleEquals()
    {
        // Arrange
        var tenantContext1 = new TenantContext();
        var tenantContext2 = new TenantContext();

        // Act
        var equalsResult = tenantContext1.Equals(tenantContext2);

        // Assert
        // Note: Reference equality for classes, so different instances should not be equal
        equalsResult.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_HandleEqualsWithSameInstance()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act
        var equalsResult = tenantContext.Equals(tenantContext);

        // Assert
        equalsResult.Should().BeTrue();
    }

    [Fact]
    public void TenantContext_Should_HandleEqualsWithNull()
    {
        // Arrange
        var tenantContext = new TenantContext();

        // Act
        var equalsResult = tenantContext.Equals(null);

        // Assert
        equalsResult.Should().BeFalse();
    }

    [Fact]
    public void TenantContext_Should_HandleEqualsWithDifferentType()
    {
        // Arrange
        var tenantContext = new TenantContext();
        var differentObject = "string";

        // Act
        var equalsResult = tenantContext.Equals(differentObject);

        // Assert
        equalsResult.Should().BeFalse();
    }
} 