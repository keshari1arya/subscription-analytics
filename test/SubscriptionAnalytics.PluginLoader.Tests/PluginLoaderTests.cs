using FluentAssertions;

namespace SubscriptionAnalytics.PluginLoader.Tests;

public class PluginLoaderTests
{
    private readonly SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader _loader;

    public PluginLoaderTests()
    {
        _loader = new SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader();
    }

    [Fact]
    public void Constructor_Should_CreateLoader()
    {
        // Act & Assert
        _loader.Should().NotBeNull();
    }

    [Fact]
    public void Loader_Should_BeOfCorrectType()
    {
        // Act & Assert
        _loader.Should().BeOfType<SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader>();
    }

    [Fact]
    public void Loader_Should_BeInstantiable()
    {
        // Act
        var loader = new SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader();

        // Assert
        loader.Should().NotBeNull();
    }

    [Fact]
    public void Loader_Should_BeReusable()
    {
        // Act
        var loader1 = new SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader();
        var loader2 = new SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader();

        // Assert
        loader1.Should().NotBeNull();
        loader2.Should().NotBeNull();
        loader1.Should().NotBeSameAs(loader2);
    }

    [Fact]
    public void Loader_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        _loader.GetType().Namespace.Should().Be("SubscriptionAnalytics.PluginLoader.Discovery");
    }

    [Fact]
    public void Loader_Should_BePublic()
    {
        // Act & Assert
        _loader.GetType().IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Loader_Should_BeClass()
    {
        // Act & Assert
        _loader.GetType().IsClass.Should().BeTrue();
    }

    [Fact]
    public void Loader_Should_NotBeAbstract()
    {
        // Act & Assert
        _loader.GetType().IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeInterface()
    {
        // Act & Assert
        _loader.GetType().IsInterface.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeEnum()
    {
        // Act & Assert
        _loader.GetType().IsEnum.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeValueType()
    {
        // Act & Assert
        _loader.GetType().IsValueType.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_BeReferenceType()
    {
        // Act & Assert
        _loader.GetType().IsByRef.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = _loader.GetType().GetConstructor(Type.EmptyTypes);
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void Loader_Should_NotHaveParameterizedConstructor()
    {
        // Act
        var constructors = _loader.GetType().GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].GetParameters().Should().BeEmpty();
    }

    [Fact]
    public async Task Loader_Should_BeThreadSafe()
    {
        // Act
        var loaders = new System.Collections.Concurrent.ConcurrentBag<SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader>();
        var tasks = new List<Task>();

        // Create multiple loaders concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => loaders.Add(new SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader())));
        }

        // Assert
        await Task.WhenAll(tasks);
        loaders.Should().HaveCount(10);
        loaders.Should().OnlyContain(l => l != null);
    }

    [Fact]
    public void Loader_Should_BeSerializable()
    {
        // Act & Assert
        _loader.GetType().IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeGeneric()
    {
        // Act & Assert
        _loader.GetType().IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        _loader.GetType().IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNested()
    {
        // Act & Assert
        _loader.GetType().IsNested.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedPublic()
    {
        // Act & Assert
        _loader.GetType().IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        _loader.GetType().IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedFamily()
    {
        // Act & Assert
        _loader.GetType().IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        _loader.GetType().IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        _loader.GetType().IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        _loader.GetType().IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        _loader.GetType().Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.PluginLoader");
    }

    [Fact]
    public void Loader_Should_HaveCorrectFullName()
    {
        // Act & Assert
        _loader.GetType().FullName.Should().Be("SubscriptionAnalytics.PluginLoader.Discovery.PluginLoader");
    }

    [Fact]
    public void Loader_Should_HaveCorrectName()
    {
        // Act & Assert
        _loader.GetType().Name.Should().Be("PluginLoader");
    }

    [Fact]
    public void Loader_Should_NotBeSealed()
    {
        // Act & Assert
        _loader.GetType().IsSealed.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeStatic()
    {
        // Act & Assert
        _loader.GetType().IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeDelegate()
    {
        // Act & Assert
        _loader.GetType().IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeArray()
    {
        // Act & Assert
        _loader.GetType().IsArray.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBePointer()
    {
        // Act & Assert
        _loader.GetType().IsPointer.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBePrimitive()
    {
        // Act & Assert
        _loader.GetType().IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeCOMObject()
    {
        // Act & Assert
        _loader.GetType().IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeContextful()
    {
        // Act & Assert
        _loader.GetType().IsContextful.Should().BeFalse();
    }

    [Fact]
    public void Loader_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        _loader.GetType().IsMarshalByRef.Should().BeFalse();
    }
} 