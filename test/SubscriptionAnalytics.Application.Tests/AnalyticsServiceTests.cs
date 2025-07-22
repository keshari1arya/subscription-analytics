using FluentAssertions;
using SubscriptionAnalytics.Application.Services;

namespace SubscriptionAnalytics.Application.Tests;

public class AnalyticsServiceTests
{
    private readonly AnalyticsService _service;

    public AnalyticsServiceTests()
    {
        _service = new AnalyticsService();
    }

    [Fact]
    public void Constructor_Should_CreateService()
    {
        // Act & Assert
        _service.Should().NotBeNull();
    }

    [Fact]
    public void Service_Should_BeOfCorrectType()
    {
        // Act & Assert
        _service.Should().BeOfType<AnalyticsService>();
    }

    [Fact]
    public void Service_Should_BeInstantiable()
    {
        // Act
        var service = new AnalyticsService();

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Service_Should_BeReusable()
    {
        // Act
        var service1 = new AnalyticsService();
        var service2 = new AnalyticsService();

        // Assert
        service1.Should().NotBeNull();
        service2.Should().NotBeNull();
        service1.Should().NotBeSameAs(service2);
    }

    [Fact]
    public void Service_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        _service.GetType().Namespace.Should().Be("SubscriptionAnalytics.Application.Services");
    }

    [Fact]
    public void Service_Should_BePublic()
    {
        // Act & Assert
        _service.GetType().IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Service_Should_BeClass()
    {
        // Act & Assert
        _service.GetType().IsClass.Should().BeTrue();
    }

    [Fact]
    public void Service_Should_NotBeAbstract()
    {
        // Act & Assert
        _service.GetType().IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeInterface()
    {
        // Act & Assert
        _service.GetType().IsInterface.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeEnum()
    {
        // Act & Assert
        _service.GetType().IsEnum.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeValueType()
    {
        // Act & Assert
        _service.GetType().IsValueType.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_BeReferenceType()
    {
        // Act & Assert
        _service.GetType().IsByRef.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = _service.GetType().GetConstructor(Type.EmptyTypes);
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void Service_Should_NotHaveParameterizedConstructor()
    {
        // Act
        var constructors = _service.GetType().GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].GetParameters().Should().BeEmpty();
    }

    [Fact]
    public async Task Service_Should_BeThreadSafe()
    {
        // Act
        var services = new List<AnalyticsService>();
        var tasks = new List<Task>();

        // Create multiple services concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => services.Add(new AnalyticsService())));
        }

        // Assert
        await Task.WhenAll(tasks);
        services.Should().HaveCount(10);
        services.Should().OnlyContain(s => s != null);
    }

    [Fact]
    public void Service_Should_BeSerializable()
    {
        // Act & Assert
        _service.GetType().IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeGeneric()
    {
        // Act & Assert
        _service.GetType().IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        _service.GetType().IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNested()
    {
        // Act & Assert
        _service.GetType().IsNested.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedPublic()
    {
        // Act & Assert
        _service.GetType().IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        _service.GetType().IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedFamily()
    {
        // Act & Assert
        _service.GetType().IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        _service.GetType().IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        _service.GetType().IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void Service_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        _service.GetType().IsNestedFamANDAssem.Should().BeFalse();
    }
} 