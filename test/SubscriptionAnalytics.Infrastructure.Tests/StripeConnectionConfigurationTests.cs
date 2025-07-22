using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Tests;

public class StripeConnectionConfigurationTests
{
    private readonly ModelBuilder _modelBuilder;
    private readonly EntityTypeBuilder<StripeConnection> _entityTypeBuilder;
    private readonly StripeConnectionConfiguration _configuration;

    public StripeConnectionConfigurationTests()
    {
        _modelBuilder = new ModelBuilder();
        _entityTypeBuilder = _modelBuilder.Entity<StripeConnection>();
        _configuration = new StripeConnectionConfiguration();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_BeInstantiable()
    {
        // Act & Assert
        _configuration.Should().NotBeNull();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ImplementIEntityTypeConfiguration()
    {
        // Act & Assert
        _configuration.Should().BeAssignableTo<IEntityTypeConfiguration<StripeConnection>>();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigurePrimaryKey()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var primaryKey = entityType!.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Should().HaveCount(1);
        primaryKey.Properties.First().Name.Should().Be("Id");
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureTenantIdUniqueIndex()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var indexes = entityType!.GetIndexes();
        var tenantIdIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "TenantId"));
        tenantIdIndex.Should().NotBeNull();
        tenantIdIndex!.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureStripeAccountIdUniqueIndex()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var indexes = entityType!.GetIndexes();
        var stripeAccountIdIndex = indexes.FirstOrDefault(i => i.Properties.Any(p => p.Name == "StripeAccountId"));
        stripeAccountIdIndex.Should().NotBeNull();
        stripeAccountIdIndex!.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureAccessTokenAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var accessTokenProperty = entityType!.FindProperty("AccessToken");
        accessTokenProperty.Should().NotBeNull();
        accessTokenProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureRefreshTokenAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var refreshTokenProperty = entityType!.FindProperty("RefreshToken");
        refreshTokenProperty.Should().NotBeNull();
        refreshTokenProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureStatusAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var statusProperty = entityType!.FindProperty("Status");
        statusProperty.Should().NotBeNull();
        statusProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureStripeAccountIdAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var stripeAccountIdProperty = entityType!.FindProperty("StripeAccountId");
        stripeAccountIdProperty.Should().NotBeNull();
        stripeAccountIdProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureConnectedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var connectedAtProperty = entityType!.FindProperty("ConnectedAt");
        connectedAtProperty.Should().NotBeNull();
        connectedAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureTenantIdAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var tenantIdProperty = entityType!.FindProperty("TenantId");
        tenantIdProperty.Should().NotBeNull();
        tenantIdProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureCreatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var createdAtProperty = entityType!.FindProperty("CreatedAt");
        createdAtProperty.Should().NotBeNull();
        createdAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureUpdatedAtAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var updatedAtProperty = entityType!.FindProperty("UpdatedAt");
        updatedAtProperty.Should().NotBeNull();
        updatedAtProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureIsDeletedAsRequired()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var isDeletedProperty = entityType!.FindProperty("IsDeleted");
        isDeletedProperty.Should().NotBeNull();
        isDeletedProperty!.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureDeletedAtAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var deletedAtProperty = entityType!.FindProperty("DeletedAt");
        deletedAtProperty.Should().NotBeNull();
        deletedAtProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureCreatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureCreatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var createdByProperty = entityType!.FindProperty("CreatedBy");
        createdByProperty.Should().NotBeNull();
        createdByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureUpdatedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureUpdatedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var updatedByProperty = entityType!.FindProperty("UpdatedBy");
        updatedByProperty.Should().NotBeNull();
        updatedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureDeletedByAsOptional()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.IsNullable.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureDeletedByMaxLength()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var deletedByProperty = entityType!.FindProperty("DeletedBy");
        deletedByProperty.Should().NotBeNull();
        deletedByProperty!.GetMaxLength().Should().Be(450);
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureTenantRelationship()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var foreignKeys = entityType!.GetForeignKeys();
        var tenantForeignKey = foreignKeys.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == "TenantId"));
        tenantForeignKey.Should().NotBeNull();
        tenantForeignKey!.PrincipalEntityType.ClrType.Should().Be(typeof(Tenant));
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureCascadeDelete()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var foreignKeys = entityType!.GetForeignKeys();
        var tenantForeignKey = foreignKeys.FirstOrDefault(fk => fk.Properties.Any(p => p.Name == "TenantId"));
        tenantForeignKey.Should().NotBeNull();
        tenantForeignKey!.DeleteBehavior.Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_ConfigureAllProperties()
    {
        // Act
        _configuration.Configure(_entityTypeBuilder);
        var model = _modelBuilder.FinalizeModel();
        var entityType = model.FindEntityType(typeof(StripeConnection));

        // Assert
        entityType.Should().NotBeNull();
        var properties = entityType!.GetProperties();
        properties.Should().NotBeEmpty();
        
        // Verify all expected properties are configured
        var propertyNames = properties.Select(p => p.Name).ToList();
        propertyNames.Should().Contain("Id");
        propertyNames.Should().Contain("AccessToken");
        propertyNames.Should().Contain("RefreshToken");
        propertyNames.Should().Contain("Status");
        propertyNames.Should().Contain("StripeAccountId");
        propertyNames.Should().Contain("ConnectedAt");
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
    public void StripeConnectionConfiguration_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var configurations = new System.Collections.Concurrent.ConcurrentBag<StripeConnectionConfiguration>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => configurations.Add(new StripeConnectionConfiguration())));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        configurations.Should().HaveCount(10);
        configurations.Should().OnlyContain(c => c != null);
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_BeReusable()
    {
        // Arrange
        var config1 = new StripeConnectionConfiguration();
        var config2 = new StripeConnectionConfiguration();

        // Act & Assert
        config1.Should().NotBeSameAs(config2);
        config1.Should().NotBeNull();
        config2.Should().NotBeNull();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveCorrectType()
    {
        // Act & Assert
        _configuration.Should().BeOfType<StripeConnectionConfiguration>();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_BePublic()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_BeClass()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsClass.Should().BeTrue();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveConfigureMethod()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).GetMethod("Configure").Should().NotBeNull();
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).Namespace.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations");
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Infrastructure");
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).FullName.Should().Be("SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations.StripeConnectionConfiguration");
    }

    [Fact]
    public void StripeConnectionConfiguration_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(StripeConnectionConfiguration).Name.Should().Be("StripeConnectionConfiguration");
    }
} 