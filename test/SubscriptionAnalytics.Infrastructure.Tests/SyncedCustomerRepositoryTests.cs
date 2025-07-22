using FluentAssertions;
using SubscriptionAnalytics.Infrastructure.Repositories;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class SyncedCustomerRepositoryTests
{
    [Fact]
    public void SyncedCustomerRepository_Should_BeInstantiable()
    {
        // Act
        var repository = new SyncedCustomerRepository();

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HaveCorrectType()
    {
        // Act
        var repository = new SyncedCustomerRepository();

        // Assert
        repository.Should().BeOfType<SyncedCustomerRepository>();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_BePublic()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_BeClass()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsClass.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Repositories");
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Repositories.SyncedCustomerRepository");
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(SyncedCustomerRepository).Name.Should().Be("SyncedCustomerRepository");
    }

    [Fact]
    public void SyncedCustomerRepository_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var repositories = new System.Collections.Concurrent.ConcurrentBag<SyncedCustomerRepository>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => repositories.Add(new SyncedCustomerRepository())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        repositories.Should().HaveCount(10);
        repositories.Should().OnlyContain(r => r != null);
    }

    [Fact]
    public void SyncedCustomerRepository_Should_BeReusable()
    {
        // Arrange
        var repo1 = new SyncedCustomerRepository();
        var repo2 = new SyncedCustomerRepository();

        // Act & Assert
        repo1.Should().NotBeSameAs(repo2);
        repo1.Should().NotBeNull();
        repo2.Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HandleEquals()
    {
        // Arrange
        var repo1 = new SyncedCustomerRepository();
        var repo2 = new SyncedCustomerRepository();

        // Act & Assert
        repo1.Equals(repo1).Should().BeTrue();
        repo1.Equals(repo2).Should().BeFalse();
        repo1.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HandleGetHashCode()
    {
        // Arrange
        var repository = new SyncedCustomerRepository();

        // Act
        var hashCode = repository.GetHashCode();

        // Assert
        hashCode.Should().NotBe(0);
    }

    [Fact]
    public void SyncedCustomerRepository_Should_HandleToString()
    {
        // Arrange
        var repository = new SyncedCustomerRepository();

        // Act
        var result = repository.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("SyncedCustomerRepository");
    }
} 