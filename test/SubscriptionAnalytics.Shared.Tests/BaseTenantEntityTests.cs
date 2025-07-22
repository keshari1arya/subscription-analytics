using FluentAssertions;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Tests;

public class BaseTenantEntityTests
{
    // Create a concrete implementation for testing
    private class TestTenantEntity : BaseTenantEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void BaseTenantEntity_Should_BeAbstract()
    {
        // Act & Assert
        typeof(BaseTenantEntity).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void BaseTenantEntity_Should_InheritFromBaseEntity()
    {
        // Act & Assert
        typeof(BaseTenantEntity).BaseType.Should().Be(typeof(BaseEntity));
    }

    [Fact]
    public void BaseTenantEntity_Should_HaveTenantIdProperty()
    {
        // Act
        var property = typeof(BaseTenantEntity).GetProperty("TenantId");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(Guid));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseTenantEntity_Should_HaveTenantProperty()
    {
        // Act
        var property = typeof(BaseTenantEntity).GetProperty("Tenant");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(Tenant));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
        property.GetGetMethod()!.IsVirtual.Should().BeTrue();
    }

    [Fact]
    public void ConcreteImplementation_Should_BeInstantiable()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.Should().NotBeNull();
        entity.Should().BeOfType<TestTenantEntity>();
        entity.Should().BeAssignableTo<BaseTenantEntity>();
        entity.Should().BeAssignableTo<BaseEntity>();
    }

    [Fact]
    public void TenantId_Should_BeSettableAndGettable()
    {
        // Arrange
        var entity = new TestTenantEntity();
        var tenantId = Guid.NewGuid();

        // Act
        entity.TenantId = tenantId;

        // Assert
        entity.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public void TenantId_Should_DefaultToEmptyGuid()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.TenantId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Tenant_Should_BeSettableAndGettable()
    {
        // Arrange
        var entity = new TestTenantEntity();
        var tenant = new Tenant { Id = Guid.NewGuid(), Name = "Test Tenant" };

        // Act
        entity.Tenant = tenant;

        // Assert
        entity.Tenant.Should().Be(tenant);
    }

    [Fact]
    public void Tenant_Should_DefaultToNull()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.Tenant.Should().BeNull();
    }

    [Fact]
    public void BaseEntity_Should_HaveIdProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("Id");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(Guid));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveCreatedAtProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("CreatedAt");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(DateTime));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveUpdatedAtProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("UpdatedAt");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(DateTime));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveIsDeletedProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("IsDeleted");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(bool));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveDeletedAtProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("DeletedAt");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(DateTime?));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveCreatedByProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("CreatedBy");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveUpdatedByProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("UpdatedBy");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_HaveDeletedByProperty()
    {
        // Act
        var property = typeof(BaseEntity).GetProperty("DeletedBy");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_DefaultToEmptyGuid()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void BaseEntity_Should_DefaultToUtcNow()
    {
        // Act
        var entity = new TestTenantEntity();
        var now = DateTime.UtcNow;

        // Assert
        entity.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        entity.UpdatedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void BaseEntity_Should_DefaultToFalseForIsDeleted()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_DefaultToNullForDeletedAt()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.DeletedAt.Should().BeNull();
    }

    [Fact]
    public void BaseEntity_Should_DefaultToEmptyStringForAuditFields()
    {
        // Act
        var entity = new TestTenantEntity();

        // Assert
        entity.CreatedBy.Should().BeEmpty();
        entity.UpdatedBy.Should().BeEmpty();
        entity.DeletedBy.Should().BeEmpty();
    }

    [Fact]
    public void Properties_Should_BeSettableAndGettable()
    {
        // Arrange
        var entity = new TestTenantEntity();
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var updatedAt = DateTime.UtcNow;
        var deletedAt = DateTime.UtcNow.AddHours(-1);
        var createdBy = "user1";
        var updatedBy = "user2";
        var deletedBy = "user3";

        // Act
        entity.Id = id;
        entity.CreatedAt = createdAt;
        entity.UpdatedAt = updatedAt;
        entity.IsDeleted = true;
        entity.DeletedAt = deletedAt;
        entity.CreatedBy = createdBy;
        entity.UpdatedBy = updatedBy;
        entity.DeletedBy = deletedBy;

        // Assert
        entity.Id.Should().Be(id);
        entity.CreatedAt.Should().Be(createdAt);
        entity.UpdatedAt.Should().Be(updatedAt);
        entity.IsDeleted.Should().BeTrue();
        entity.DeletedAt.Should().Be(deletedAt);
        entity.CreatedBy.Should().Be(createdBy);
        entity.UpdatedBy.Should().Be(updatedBy);
        entity.DeletedBy.Should().Be(deletedBy);
    }

    [Fact]
    public void BaseEntity_Should_BeAbstract()
    {
        // Act & Assert
        typeof(BaseEntity).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_BeClass()
    {
        // Act & Assert
        typeof(BaseEntity).IsClass.Should().BeTrue();
    }

    [Fact]
    public void BaseEntity_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(BaseEntity).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(BaseEntity).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(BaseEntity).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(BaseEntity).IsByRef.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(BaseEntity).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_NotBeNested()
    {
        // Act & Assert
        typeof(BaseEntity).IsNested.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(BaseEntity).Namespace.Should().Be("SubscriptionAnalytics.Shared.Entities");
    }

    [Fact]
    public void BaseEntity_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(BaseEntity).FullName.Should().Be("SubscriptionAnalytics.Shared.Entities.BaseEntity");
    }

    [Fact]
    public void BaseEntity_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(BaseEntity).Name.Should().Be("BaseEntity");
    }

    [Fact]
    public void BaseTenantEntity_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(BaseTenantEntity).Namespace.Should().Be("SubscriptionAnalytics.Shared.Entities");
    }

    [Fact]
    public void BaseTenantEntity_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(BaseTenantEntity).FullName.Should().Be("SubscriptionAnalytics.Shared.Entities.BaseTenantEntity");
    }

    [Fact]
    public void BaseTenantEntity_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(BaseTenantEntity).Name.Should().Be("BaseTenantEntity");
    }
} 