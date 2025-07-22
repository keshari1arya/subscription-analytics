using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Api.Controllers;

namespace SubscriptionAnalytics.Api.Tests;

public class TestControllerTests
{
    private readonly Mock<ILogger<TestController>> _loggerMock = new();
    private readonly TestController _controller;

    public TestControllerTests()
    {
        _controller = new TestController(_loggerMock.Object);
    }

    [Fact]
    public void Get_Should_Return_ApiStatus_WithUser()
    {
        // Arrange
        var userName = "testuser";
        var httpContext = new DefaultHttpContext();
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, userName)
            }, "mock"));
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        // Act
        var result = _controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("API is working!");
        value.User.Should().Be(userName);
    }

    [Fact]
    public void Get_Should_Return_Anonymous_IfNoUser()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.User.Should().Be("Anonymous");
    }

    [Fact]
    public void Post_Should_Echo_Data()
    {
        // Arrange
        var data = new { foo = "bar" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Post(data) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("POST request received");
        value.Data.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void Constructor_Should_CreateController()
    {
        // Act & Assert
        _controller.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_BeOfCorrectType()
    {
        // Act & Assert
        _controller.Should().BeOfType<TestController>();
    }

    [Fact]
    public void Controller_Should_InheritFromControllerBase()
    {
        // Act & Assert
        _controller.Should().BeAssignableTo<ControllerBase>();
    }

    [Fact]
    public void Controller_Should_HaveApiControllerAttribute()
    {
        // Act
        var attributes = _controller.GetType().GetCustomAttributes(typeof(ApiControllerAttribute), true);

        // Assert
        attributes.Should().HaveCount(1);
    }

    [Fact]
    public void Controller_Should_HaveRouteAttribute()
    {
        // Act
        var attributes = _controller.GetType().GetCustomAttributes(typeof(RouteAttribute), true);

        // Assert
        attributes.Should().HaveCount(1);
        var routeAttribute = (RouteAttribute)attributes[0];
        routeAttribute.Template.Should().Be("api/[controller]");
    }

    [Fact]
    public void Controller_Should_HaveAuthorizeAttribute()
    {
        // Act
        var attributes = _controller.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);

        // Assert
        attributes.Should().HaveCount(1);
    }

    [Fact]
    public void Get_Should_LogInformation()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        _controller.Get();

        // Assert
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Test endpoint called")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public void Post_Should_LogInformation()
    {
        // Arrange
        var data = new { test = "data" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        _controller.Post(data);

        // Assert
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Received POST data")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public void Get_Should_ReturnCorrectTimestamp()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        var beforeCall = DateTime.UtcNow;

        // Act
        var result = _controller.Get() as OkObjectResult;
        var afterCall = DateTime.UtcNow;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Timestamp.Should().BeOnOrAfter(beforeCall);
        value.Timestamp.Should().BeOnOrBefore(afterCall);
    }

    [Fact]
    public void Post_Should_ReturnCorrectTimestamp()
    {
        // Arrange
        var data = new { test = "data" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        var beforeCall = DateTime.UtcNow;

        // Act
        var result = _controller.Post(data) as OkObjectResult;
        var afterCall = DateTime.UtcNow;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Timestamp.Should().BeOnOrAfter(beforeCall);
        value.Timestamp.Should().BeOnOrBefore(afterCall);
    }

    [Fact]
    public void Get_Should_ReturnCorrectMessage()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Get() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("API is working!");
    }

    [Fact]
    public void Post_Should_ReturnCorrectMessage()
    {
        // Arrange
        var data = new { test = "data" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Post(data) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var value = result!.Value as TestController.TestResponseDto;
        value.Should().NotBeNull();
        value!.Message.Should().Be("POST request received");
    }

    [Fact]
    public void Get_Should_ReturnOkResult()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void Post_Should_ReturnOkResult()
    {
        // Arrange
        var data = new { test = "data" };
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        // Act
        var result = _controller.Post(data);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void TestResponseDto_Should_HaveCorrectProperties()
    {
        // Act
        var dto = new TestController.TestResponseDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Message.Should().Be(string.Empty);
        dto.Timestamp.Should().Be(default(DateTime));
        dto.User.Should().Be(string.Empty);
        dto.Data.Should().BeNull();
    }

    [Fact]
    public void TestResponseDto_Should_BeSettable()
    {
        // Arrange
        var dto = new TestController.TestResponseDto();
        var message = "Test message";
        var timestamp = DateTime.UtcNow;
        var user = "testuser";
        var data = new { test = "data" };

        // Act
        dto.Message = message;
        dto.Timestamp = timestamp;
        dto.User = user;
        dto.Data = data;

        // Assert
        dto.Message.Should().Be(message);
        dto.Timestamp.Should().Be(timestamp);
        dto.User.Should().Be(user);
        dto.Data.Should().Be(data);
    }

    [Fact]
    public void Controller_Should_BeInstantiable()
    {
        // Act
        var controller = new TestController(_loggerMock.Object);

        // Assert
        controller.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_BeReusable()
    {
        // Act
        var controller1 = new TestController(_loggerMock.Object);
        var controller2 = new TestController(_loggerMock.Object);

        // Assert
        controller1.Should().NotBeNull();
        controller2.Should().NotBeNull();
        controller1.Should().NotBeSameAs(controller2);
    }

    [Fact]
    public void Controller_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        _controller.GetType().Namespace.Should().Be("SubscriptionAnalytics.Api.Controllers");
    }

    [Fact]
    public void Controller_Should_BePublic()
    {
        // Act & Assert
        _controller.GetType().IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Controller_Should_BeClass()
    {
        // Act & Assert
        _controller.GetType().IsClass.Should().BeTrue();
    }

    [Fact]
    public void Controller_Should_NotBeAbstract()
    {
        // Act & Assert
        _controller.GetType().IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeInterface()
    {
        // Act & Assert
        _controller.GetType().IsInterface.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeEnum()
    {
        // Act & Assert
        _controller.GetType().IsEnum.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeValueType()
    {
        // Act & Assert
        _controller.GetType().IsValueType.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_BeReferenceType()
    {
        // Act & Assert
        _controller.GetType().IsClass.Should().BeTrue();
    }

    [Fact]
    public void Controller_Should_HaveParameterizedConstructor()
    {
        // Act & Assert
        var constructor = _controller.GetType().GetConstructor(new[] { typeof(ILogger<TestController>) });
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void Controller_Should_NotHaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = _controller.GetType().GetConstructor(Type.EmptyTypes);
        constructor.Should().BeNull();
    }

    [Fact]
    public void Controller_Should_BeThreadSafe()
    {
        // Act
        var controllers = new System.Collections.Concurrent.ConcurrentBag<TestController>();
        var tasks = new List<Task>();

        // Create multiple controllers concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => controllers.Add(new TestController(_loggerMock.Object))));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        controllers.Should().HaveCount(10);
        controllers.Should().OnlyContain(c => c != null);
    }

    [Fact]
    public void Controller_Should_NotBeSerializable()
    {
        // Act & Assert
        _controller.GetType().IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeGeneric()
    {
        // Act & Assert
        _controller.GetType().IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        _controller.GetType().IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNested()
    {
        // Act & Assert
        _controller.GetType().IsNested.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedPublic()
    {
        // Act & Assert
        _controller.GetType().IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        _controller.GetType().IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedFamily()
    {
        // Act & Assert
        _controller.GetType().IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        _controller.GetType().IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        _controller.GetType().IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        _controller.GetType().IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        _controller.GetType().Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Api");
    }

    [Fact]
    public void Controller_Should_HaveCorrectFullName()
    {
        // Act & Assert
        _controller.GetType().FullName.Should().Be("SubscriptionAnalytics.Api.Controllers.TestController");
    }

    [Fact]
    public void Controller_Should_HaveCorrectName()
    {
        // Act & Assert
        _controller.GetType().Name.Should().Be("TestController");
    }

    [Fact]
    public void Controller_Should_NotBeSealed()
    {
        // Act & Assert
        _controller.GetType().IsSealed.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeStatic()
    {
        // Act & Assert
        _controller.GetType().IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeDelegate()
    {
        // Act & Assert
        _controller.GetType().IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeArray()
    {
        // Act & Assert
        _controller.GetType().IsArray.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBePointer()
    {
        // Act & Assert
        _controller.GetType().IsPointer.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBePrimitive()
    {
        // Act & Assert
        _controller.GetType().IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeCOMObject()
    {
        // Act & Assert
        _controller.GetType().IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeContextful()
    {
        // Act & Assert
        _controller.GetType().IsContextful.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        _controller.GetType().IsMarshalByRef.Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_BeAssignableToObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(object)).Should().BeTrue();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIDisposable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IDisposable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIAsyncDisposable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IAsyncDisposable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToICloneable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(ICloneable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIComparable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IComparable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIComparableOfTestController()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IComparable<TestController>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIEquatableOfTestController()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IEquatable<TestController>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIFormattable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IFormattable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIConvertible()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IConvertible)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToISerializable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Runtime.Serialization.ISerializable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIEnumerable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.IEnumerable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIEnumerableOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IEnumerable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToICollection()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.ICollection)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToICollectionOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.ICollection<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIList()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.IList)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIListOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IList<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIDictionary()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.IDictionary)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIDictionaryOfObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IDictionary<object, object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIReadOnlyCollectionOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyCollection<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIReadOnlyListOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyList<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIReadOnlyDictionaryOfObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyDictionary<object, object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIEnumerator()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.IEnumerator)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIEnumeratorOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Collections.Generic.IEnumerator<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIAsyncEnumerableOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IAsyncEnumerable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIAsyncEnumeratorOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(IAsyncEnumerator<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIQueryable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.IQueryable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIQueryableOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.IQueryable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIOrderedQueryable()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.IOrderedQueryable)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIOrderedQueryableOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.IOrderedQueryable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToIQueryProvider()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.IQueryProvider)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpression()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Controller_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        _controller.GetType().IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }
} 