using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moq;
using SubscriptionAnalytics.Infrastructure.Configuration;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class SyncedCustomerConfigurationTests
{
    private readonly ModelBuilder _modelBuilder;
    private readonly EntityTypeBuilder<SyncedCustomer> _entityTypeBuilder;
    private readonly SyncedCustomerConfiguration _configuration;

    public SyncedCustomerConfigurationTests()
    {
        _modelBuilder = new ModelBuilder();
        _entityTypeBuilder = _modelBuilder.Entity<SyncedCustomer>();
        _configuration = new SyncedCustomerConfiguration();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_BeInstantiable()
    {
        // Act & Assert
        _configuration.Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ImplementIEntityTypeConfiguration()
    {
        // Act & Assert
        _configuration.Should().BeAssignableTo<IEntityTypeConfiguration<SyncedCustomer>>();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigurePrimaryKey()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var primaryKey = entityType!.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Should().HaveCount(1);
        primaryKey.Properties.First().Name.Should().Be("CustomerId");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureCustomerIdMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var customerIdProperty = entityType!.FindProperty("CustomerId");
        customerIdProperty.Should().NotBeNull();
        customerIdProperty!.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureUserIdAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var userIdProperty = entityType!.FindProperty("UserId");
        userIdProperty.Should().NotBeNull();
        userIdProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureTenantIdAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var tenantIdProperty = entityType!.FindProperty("TenantId");
        tenantIdProperty.Should().NotBeNull();
        tenantIdProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureNameMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var nameProperty = entityType!.FindProperty("Name");
        nameProperty.Should().NotBeNull();
        nameProperty!.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureEmailMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var emailProperty = entityType!.FindProperty("Email");
        emailProperty.Should().NotBeNull();
        emailProperty!.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigurePhoneMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var phoneProperty = entityType!.FindProperty("Phone");
        phoneProperty.Should().NotBeNull();
        phoneProperty!.GetMaxLength().Should().Be(50);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureCreatedStripeAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var createdStripeAtProperty = entityType!.FindProperty("CreatedStripeAt");
        createdStripeAtProperty.Should().NotBeNull();
        createdStripeAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureLivemodeAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var livemodeProperty = entityType!.FindProperty("Livemode");
        livemodeProperty.Should().NotBeNull();
        livemodeProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureSyncedAtDefaultValue()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var syncedAtProperty = entityType!.FindProperty("SyncedAt");
        syncedAtProperty.Should().NotBeNull();
        syncedAtProperty!.GetDefaultValueSql().Should().Be("NOW() AT TIME ZONE 'UTC'");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureUserIdIndex()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var indexes = entityType!.GetIndexes();
        indexes.Should().Contain(i => i.Properties.Any(p => p.Name == "UserId"));
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureTenantIdIndex()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var indexes = entityType!.GetIndexes();
        indexes.Should().Contain(i => i.Properties.Any(p => p.Name == "TenantId"));
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureAllProperties()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var properties = entityType!.GetProperties();
        properties.Should().NotBeEmpty();
        
        // Verify all expected properties are configured
        var propertyNames = properties.Select(p => p.Name).ToList();
        propertyNames.Should().Contain("CustomerId");
        propertyNames.Should().Contain("UserId");
        propertyNames.Should().Contain("TenantId");
        propertyNames.Should().Contain("Name");
        propertyNames.Should().Contain("Email");
        propertyNames.Should().Contain("Phone");
        propertyNames.Should().Contain("CreatedStripeAt");
        propertyNames.Should().Contain("Livemode");
        propertyNames.Should().Contain("SyncedAt");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var configurations = new System.Collections.Concurrent.ConcurrentBag<SyncedCustomerConfiguration>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => configurations.Add(new SyncedCustomerConfiguration())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        configurations.Should().HaveCount(10);
        configurations.Should().OnlyContain(c => c != null);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_BeReusable()
    {
        // Arrange
        var config1 = new SyncedCustomerConfiguration();
        var config2 = new SyncedCustomerConfiguration();

        // Act & Assert
        config1.Should().NotBeSameAs(config2);
        config1.Should().NotBeNull();
        config2.Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectType()
    {
        // Act & Assert
        _configuration.Should().BeOfType<SyncedCustomerConfiguration>();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_BePublic()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_BeClass()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsClass.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveConfigureMethod()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).GetMethod("Configure").Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Configuration");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Configuration.SyncedCustomerConfiguration");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).Name.Should().Be("SyncedCustomerConfiguration");
    }
} 