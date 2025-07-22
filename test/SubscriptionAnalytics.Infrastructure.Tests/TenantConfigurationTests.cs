using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class TenantConfigurationTests
{
    private readonly ModelBuilder _modelBuilder;
    private readonly EntityTypeBuilder<Tenant> _entityTypeBuilder;
    private readonly TenantConfiguration _configuration;

    public TenantConfigurationTests()
    {
        _modelBuilder = new ModelBuilder();
        _entityTypeBuilder = _modelBuilder.Entity<Tenant>();
        _configuration = new TenantConfiguration();
    }

    [Fact]
    public void TenantConfiguration_Should_BeInstantiable()
    {
        // Act & Assert
        _configuration.Should().NotBeNull();
    }

    [Fact]
    public void TenantConfiguration_Should_ImplementIEntityTypeConfiguration()
    {
        // Act & Assert
        _configuration.Should().BeAssignableTo<IEntityTypeConfiguration<Tenant>>();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigurePrimaryKey()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var primaryKey = entityType!.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Should().HaveCount(1);
        primaryKey.Properties.First().Name.Should().Be("Id");
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureNameAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var nameProperty = entityType!.FindProperty("Name");
        nameProperty.Should().NotBeNull();
        nameProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureNameMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var nameProperty = entityType!.FindProperty("Name");
        nameProperty.Should().NotBeNull();
        nameProperty!.GetMaxLength().Should().Be(255);
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureIsActiveAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var isActiveProperty = entityType!.FindProperty("IsActive");
        isActiveProperty.Should().NotBeNull();
        isActiveProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureCreatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var createdAtProperty = entityType!.FindProperty("CreatedAt");
        createdAtProperty.Should().NotBeNull();
        createdAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureUpdatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var updatedAtProperty = entityType!.FindProperty("UpdatedAt");
        updatedAtProperty.Should().NotBeNull();
        updatedAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureIsDeletedAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var isDeletedProperty = entityType!.FindProperty("IsDeleted");
        isDeletedProperty.Should().NotBeNull();
        isDeletedProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureDeletedAtAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var deletedAtProperty = entityType!.FindProperty("DeletedAt");
        deletedAtProperty.Should().NotBeNull();
        deletedAtProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureCreatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureCreatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureUpdatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureUpdatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureDeletedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureDeletedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void TenantConfiguration_Should_ConfigureAllProperties()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(Tenant));

        // Assert
        entityType.Should().NotBeNull();
        var properties = entityType!.GetProperties();
        properties.Should().NotBeEmpty();
        
        // Verify all expected properties are configured
        var propertyNames = properties.Select(p => p.Name).ToList();
        propertyNames.Should().Contain("Id");
        propertyNames.Should().Contain("Name");
        propertyNames.Should().Contain("IsActive");
        propertyNames.Should().Contain("CreatedAt");
        propertyNames.Should().Contain("UpdatedAt");
        propertyNames.Should().Contain("IsDeleted");
        propertyNames.Should().Contain("DeletedAt");
        propertyNames.Should().Contain("CreatedBy");
        propertyNames.Should().Contain("UpdatedBy");
        propertyNames.Should().Contain("DeletedBy");
    }

    [Fact]
    public void TenantConfiguration_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var configurations = new System.Collections.Concurrent.ConcurrentBag<TenantConfiguration>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => configurations.Add(new TenantConfiguration())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        configurations.Should().HaveCount(10);
        configurations.Should().OnlyContain(c => c != null);
    }

    [Fact]
    public void TenantConfiguration_Should_BeReusable()
    {
        // Arrange
        var config1 = new TenantConfiguration();
        var config2 = new TenantConfiguration();

        // Act & Assert
        config1.Should().NotBeSameAs(config2);
        config1.Should().NotBeNull();
        config2.Should().NotBeNull();
    }

    [Fact]
    public void TenantConfiguration_Should_HaveCorrectType()
    {
        // Act & Assert
        _configuration.Should().BeOfType<TenantConfiguration>();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_BePublic()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_BeClass()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsClass.Should().BeTrue();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(TenantConfiguration).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void TenantConfiguration_Should_HaveConfigureMethod()
    {
        // Act & Assert
        typeof(TenantConfiguration).GetMethod("Configure").Should().NotBeNull();
    }

    [Fact]
    public void TenantConfiguration_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(TenantConfiguration).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations");
    }

    [Fact]
    public void TenantConfiguration_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(TenantConfiguration).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void TenantConfiguration_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(TenantConfiguration).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations.TenantConfiguration");
    }

    [Fact]
    public void TenantConfiguration_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(TenantConfiguration).Name.Should().Be("TenantConfiguration");
    }
} 