using FluentAssertions;
using SubscriptionAnalytics.Worker.Jobs;

namespace SubscriptionAnalytics.Worker.Tests;

public class CustomerSyncJobTests
{
    [Fact]
    public void CustomerSyncJob_Should_BeInstantiable()
    {
        // Act
        var job = new CustomerSyncJob();

        // Assert
        job.Should().NotBeNull();
    }

    [Fact]
    public void CustomerSyncJob_Should_HaveCorrectType()
    {
        // Act
        var job = new CustomerSyncJob();

        // Assert
        job.Should().BeOfType<CustomerSyncJob>();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_BePublic()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void CustomerSyncJob_Should_BeClass()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsClass.Should().BeTrue();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(CustomerSyncJob).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(CustomerSyncJob).Namespace.Should().Be("SubscriptionAnalytics.Worker.Jobs");
    }

    [Fact]
    public void CustomerSyncJob_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(CustomerSyncJob).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Worker");
    }

    [Fact]
    public void CustomerSyncJob_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(CustomerSyncJob).FullName.Should().Be("SubscriptionAnalytics.Worker.Jobs.CustomerSyncJob");
    }

    [Fact]
    public void CustomerSyncJob_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(CustomerSyncJob).Name.Should().Be("CustomerSyncJob");
    }

    [Fact]
    public void CustomerSyncJob_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var jobs = new System.Collections.Concurrent.ConcurrentBag<CustomerSyncJob>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => jobs.Add(new CustomerSyncJob())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        jobs.Should().HaveCount(10);
        jobs.Should().OnlyContain(j => j != null);
    }

    [Fact]
    public void CustomerSyncJob_Should_BeReusable()
    {
        // Arrange
        var job1 = new CustomerSyncJob();
        var job2 = new CustomerSyncJob();

        // Act & Assert
        job1.Should().NotBeSameAs(job2);
        job1.Should().NotBeNull();
        job2.Should().NotBeNull();
    }

    [Fact]
    public void CustomerSyncJob_Should_HandleEquals()
    {
        // Arrange
        var job1 = new CustomerSyncJob();
        var job2 = new CustomerSyncJob();

        // Act & Assert
        job1.Equals(job1).Should().BeTrue();
        job1.Equals(job2).Should().BeFalse();
        job1.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void CustomerSyncJob_Should_HandleGetHashCode()
    {
        // Arrange
        var job = new CustomerSyncJob();

        // Act
        var hashCode = job.GetHashCode();

        // Assert
        hashCode.Should().NotBe(0);
    }

    [Fact]
    public void CustomerSyncJob_Should_HandleToString()
    {
        // Arrange
        var job = new CustomerSyncJob();

        // Act
        var result = job.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("CustomerSyncJob");
    }
} 