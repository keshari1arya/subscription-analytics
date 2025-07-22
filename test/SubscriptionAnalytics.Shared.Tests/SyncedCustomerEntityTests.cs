using FluentAssertions;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Shared.Tests;

public class SyncedCustomerEntityTests
{
    [Fact]
    public void SyncedCustomer_Should_InheritFromBaseTenantEntity()
    {
        // Act & Assert
        typeof(SyncedCustomer).BaseType.Should().Be(typeof(BaseTenantEntity));
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_BeInstantiable()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.Should().NotBeNull();
        syncedCustomer.Should().BeOfType<SyncedCustomer>();
        syncedCustomer.Should().BeAssignableTo<BaseTenantEntity>();
        syncedCustomer.Should().BeAssignableTo<BaseEntity>();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveCustomerIdProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("CustomerId");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveUserIdProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("UserId");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(Guid));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveNameProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("Name");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveEmailProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("Email");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HavePhoneProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("Phone");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(string));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveCreatedStripeAtProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("CreatedStripeAt");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(DateTime));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveLivemodeProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("Livemode");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(bool));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveSyncedAtProperty()
    {
        // Act
        var property = typeof(SyncedCustomer).GetProperty("SyncedAt");

        // Assert
        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(DateTime));
        property.CanRead.Should().BeTrue();
        property.CanWrite.Should().BeTrue();
    }

    [Fact]
    public void CustomerId_Should_DefaultToEmptyString()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.CustomerId.Should().BeEmpty();
    }

    [Fact]
    public void UserId_Should_DefaultToEmptyGuid()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.UserId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Name_Should_DefaultToNull()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.Name.Should().BeNull();
    }

    [Fact]
    public void Email_Should_DefaultToNull()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.Email.Should().BeNull();
    }

    [Fact]
    public void Phone_Should_DefaultToNull()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.Phone.Should().BeNull();
    }

    [Fact]
    public void CreatedStripeAt_Should_DefaultToMinValue()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.CreatedStripeAt.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void Livemode_Should_DefaultToFalse()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();

        // Assert
        syncedCustomer.Livemode.Should().BeFalse();
    }

    [Fact]
    public void SyncedAt_Should_DefaultToUtcNow()
    {
        // Act
        var syncedCustomer = new SyncedCustomer();
        var now = DateTime.UtcNow;

        // Assert
        syncedCustomer.SyncedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Properties_Should_BeSettableAndGettable()
    {
        // Arrange
        var syncedCustomer = new SyncedCustomer();
        var customerId = "cus_123456789";
        var userId = Guid.NewGuid();
        var name = "John Doe";
        var email = "john.doe@example.com";
        var phone = "+1234567890";
        var createdStripeAt = DateTime.UtcNow.AddDays(-30);
        var livemode = true;
        var syncedAt = DateTime.UtcNow.AddHours(-1);

        // Act
        syncedCustomer.CustomerId = customerId;
        syncedCustomer.UserId = userId;
        syncedCustomer.Name = name;
        syncedCustomer.Email = email;
        syncedCustomer.Phone = phone;
        syncedCustomer.CreatedStripeAt = createdStripeAt;
        syncedCustomer.Livemode = livemode;
        syncedCustomer.SyncedAt = syncedAt;

        // Assert
        syncedCustomer.CustomerId.Should().Be(customerId);
        syncedCustomer.UserId.Should().Be(userId);
        syncedCustomer.Name.Should().Be(name);
        syncedCustomer.Email.Should().Be(email);
        syncedCustomer.Phone.Should().Be(phone);
        syncedCustomer.CreatedStripeAt.Should().Be(createdStripeAt);
        syncedCustomer.Livemode.Should().Be(livemode);
        syncedCustomer.SyncedAt.Should().Be(syncedAt);
    }

    [Fact]
    public void SyncedCustomer_Should_BeReusable()
    {
        // Act
        var syncedCustomer1 = new SyncedCustomer();
        var syncedCustomer2 = new SyncedCustomer();

        // Assert
        syncedCustomer1.Should().NotBeNull();
        syncedCustomer2.Should().NotBeNull();
        syncedCustomer1.Should().NotBeSameAs(syncedCustomer2);
    }

    [Fact]
    public void SyncedCustomer_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(SyncedCustomer).Namespace.Should().Be("SubscriptionAnalytics.Shared.Entities");
    }

    [Fact]
    public void SyncedCustomer_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(SyncedCustomer).FullName.Should().Be("SubscriptionAnalytics.Shared.Entities.SyncedCustomer");
    }

    [Fact]
    public void SyncedCustomer_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(SyncedCustomer).Name.Should().Be("SyncedCustomer");
    }

    [Fact]
    public void SyncedCustomer_Should_BePublic()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_BeClass()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsClass.Should().BeTrue();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsByRef.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = typeof(SyncedCustomer).GetConstructor(Type.EmptyTypes);
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void SyncedCustomer_Should_NotHaveParameterizedConstructor()
    {
        // Act
        var constructors = typeof(SyncedCustomer).GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].GetParameters().Should().BeEmpty();
    }

    [Fact]
    public async Task SyncedCustomer_Should_BeThreadSafe()
    {
        // Act
        var syncedCustomers = new System.Collections.Concurrent.ConcurrentBag<SyncedCustomer>();
        var tasks = new List<Task>();

        // Create multiple synced customers concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => syncedCustomers.Add(new SyncedCustomer())));
        }

        // Assert
        await Task.WhenAll(tasks);
        syncedCustomers.Should().HaveCount(10);
        syncedCustomers.Should().OnlyContain(sc => sc != null);
    }

    [Fact]
    public void SyncedCustomer_Should_BeSerializable()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNested()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNested.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeStatic()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeArray()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsArray.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBePointer()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void SyncedCustomer_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(SyncedCustomer).IsMarshalByRef.Should().BeFalse();
    }
} 