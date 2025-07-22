using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class SyncedCustomerEntityConfigurationTests
{
    private readonly ModelBuilder _modelBuilder;
    private readonly EntityTypeBuilder<SyncedCustomer> _entityTypeBuilder;
    private readonly SyncedCustomerConfiguration _configuration;

    public SyncedCustomerEntityConfigurationTests()
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
    public void SyncedCustomerConfiguration_Should_ConfigureCustomerIdAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var customerIdProperty = entityType!.FindProperty("CustomerId");
        customerIdProperty.Should().NotBeNull();
        customerIdProperty!.IsNullable.Should().BeFalse();
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
    public void SyncedCustomerConfiguration_Should_ConfigureSyncedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var syncedAtProperty = entityType!.FindProperty("SyncedAt");
        syncedAtProperty.Should().NotBeNull();
        syncedAtProperty!.IsNullable.Should().BeFalse();
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
    public void SyncedCustomerConfiguration_Should_ConfigureCreatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var createdAtProperty = entityType!.FindProperty("CreatedAt");
        createdAtProperty.Should().NotBeNull();
        createdAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureUpdatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var updatedAtProperty = entityType!.FindProperty("UpdatedAt");
        updatedAtProperty.Should().NotBeNull();
        updatedAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureIsDeletedAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var isDeletedProperty = entityType!.FindProperty("IsDeleted");
        isDeletedProperty.Should().NotBeNull();
        isDeletedProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureDeletedAtAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var deletedAtProperty = entityType!.FindProperty("DeletedAt");
        deletedAtProperty.Should().NotBeNull();
        deletedAtProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureCreatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureCreatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureUpdatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureUpdatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureDeletedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureDeletedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureTenantRelationship()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var foreignKeys = entityType!.GetForeignKeys();
        var tenantForeignKey = foreignKeys.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == "TenantId"));
        tenantForeignKey.Should().NotBeNull();
        tenantForeignKey!.PrincipalEntityType.ClrType.Should().Be(typeof(Tenant));
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_ConfigureCascadeDelete()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(SyncedCustomer));

        // Assert
        entityType.Should().NotBeNull();
        var foreignKeys = entityType!.GetForeignKeys();
        var tenantForeignKey = foreignKeys.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == "TenantId"));
        tenantForeignKey.Should().NotBeNull();
        tenantForeignKey!.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
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
        propertyNames.Should().Contain("SyncedAt");
        propertyNames.Should().Contain("TenantId");
        propertyNames.Should().Contain("CreatedAt");
        propertyNames.Should().Contain("UpdatedAt");
        propertyNames.Should().Contain("IsDeleted");
        propertyNames.Should().Contain("DeletedAt");
        propertyNames.Should().Contain("CreatedBy");
        propertyNames.Should().Contain("UpdatedBy");
        propertyNames.Should().Contain("DeletedBy");
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
        typeof(SyncedCustomerConfiguration).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations");
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
        typeof(SyncedCustomerConfiguration).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations.SyncedCustomerConfiguration");
    }

    [Fact]
    public void SyncedCustomerConfiguration_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(SyncedCustomerConfiguration).Name.Should().Be("SyncedCustomerConfiguration");
    }
} 