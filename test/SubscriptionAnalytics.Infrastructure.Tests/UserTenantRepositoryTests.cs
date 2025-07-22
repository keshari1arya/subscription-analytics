using FluentAssertions;
using SubscriptionAnalytics.Infrastructure.Repositories;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class UserTenantRepositoryTests
{
    [Fact]
    public void UserTenantRepository_Should_BeInstantiable()
    {
        // Act
        var repository = new UserTenantRepository();

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void UserTenantRepository_Should_HaveCorrectType()
    {
        // Act
        var repository = new UserTenantRepository();

        // Assert
        repository.Should().BeOfType<UserTenantRepository>();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_BePublic()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void UserTenantRepository_Should_BeClass()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsClass.Should().BeTrue();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(UserTenantRepository).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(UserTenantRepository).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Repositories");
    }

    [Fact]
    public void UserTenantRepository_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(UserTenantRepository).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void UserTenantRepository_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(UserTenantRepository).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Repositories.UserTenantRepository");
    }

    [Fact]
    public void UserTenantRepository_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(UserTenantRepository).Name.Should().Be("UserTenantRepository");
    }

    [Fact]
    public void UserTenantRepository_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var repositories = new System.Collections.Concurrent.ConcurrentBag<UserTenantRepository>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => repositories.Add(new UserTenantRepository())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        repositories.Should().HaveCount(10);
        repositories.Should().OnlyContain(r => r != null);
    }

    [Fact]
    public void UserTenantRepository_Should_BeReusable()
    {
        // Arrange
        var repo1 = new UserTenantRepository();
        var repo2 = new UserTenantRepository();

        // Act & Assert
        repo1.Should().NotBeSameAs(repo2);
        repo1.Should().NotBeNull();
        repo2.Should().NotBeNull();
    }

    [Fact]
    public void UserTenantRepository_Should_HandleEquals()
    {
        // Arrange
        var repo1 = new UserTenantRepository();
        var repo2 = new UserTenantRepository();

        // Act & Assert
        repo1.Equals(repo1).Should().BeTrue();
        repo1.Equals(repo2).Should().BeFalse();
        repo1.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void UserTenantRepository_Should_HandleGetHashCode()
    {
        // Arrange
        var repository = new UserTenantRepository();

        // Act
        var hashCode = repository.GetHashCode();

        // Assert
        hashCode.Should().NotBe(0);
    }

    [Fact]
    public void UserTenantRepository_Should_HandleToString()
    {
        // Arrange
        var repository = new UserTenantRepository();

        // Act
        var result = repository.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("UserTenantRepository");
    }
} 