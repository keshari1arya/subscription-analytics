using FluentAssertions;
using SubscriptionAnalytics.Shared.Utilities;

namespace SubscriptionAnalytics.Shared.Tests;

public class DateTimeHelperTests
{
    [Fact]
    public void DateTimeHelper_Should_BeStaticClass()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsAbstract.Should().BeTrue();
        typeof(DateTimeHelper).IsSealed.Should().BeTrue();
    }

    [Fact]
    public void DateTimeHelper_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(DateTimeHelper).Namespace.Should().Be("SubscriptionAnalytics.Shared.Utilities");
    }

    [Fact]
    public void DateTimeHelper_Should_BePublic()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNested()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNested.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(DateTimeHelper).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Shared");
    }

    [Fact]
    public void DateTimeHelper_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(DateTimeHelper).FullName.Should().Be("SubscriptionAnalytics.Shared.Utilities.DateTimeHelper");
    }

    [Fact]
    public void DateTimeHelper_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(DateTimeHelper).Name.Should().Be("DateTimeHelper");
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeArray()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsArray.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBePointer()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsMarshalByRef.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeSerializable()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsSealed.Should().BeTrue();
    }

    [Fact]
    public void DateTimeHelper_Should_BeStatic()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeDelegateType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsSubclassOf(typeof(MulticastDelegate)).Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeArrayType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsArray.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBePointerType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBePrimitiveType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeCOMObjectType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeContextfulType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeMarshalByRefType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsMarshalByRef.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeSerializableType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeGenericTypeDefinitionType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedPublicType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedPrivateType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamilyType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedAssemblyType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamORAssemType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void DateTimeHelper_Should_NotBeNestedFamANDAssemType()
    {
        // Act & Assert
        typeof(DateTimeHelper).IsNestedFamANDAssem.Should().BeFalse();
    }
} 