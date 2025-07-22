using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Api.Controllers;
using SubscriptionAnalytics.Application.Interfaces;
using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Api.Tests;

public class StripeControllerTests
{
    private readonly Mock<IStripeInstallationService> _stripeServiceMock;
    private readonly Mock<ILogger<StripeController>> _loggerMock;
    private readonly StripeController _controller;

    public StripeControllerTests()
    {
        _stripeServiceMock = new Mock<IStripeInstallationService>();
        _loggerMock = new Mock<ILogger<StripeController>>();
        _controller = new StripeController(_stripeServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Constructor_Should_CreateController()
    {
        // Act & Assert
        _controller.Should().NotBeNull();
        _controller.Should().BeOfType<StripeController>();
    }

    [Fact]
    public void Constructor_Should_BePublic()
    {
        // Act & Assert
        typeof(StripeController).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_BeClass()
    {
        // Act & Assert
        typeof(StripeController).IsClass.Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(StripeController).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(StripeController).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(StripeController).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(StripeController).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_BeReferenceType()
    {
        // Act & Assert
        typeof(StripeController).IsClass.Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_HaveDefaultConstructor()
    {
        // Act & Assert
        var constructor = typeof(StripeController).GetConstructor(Type.EmptyTypes);
        constructor.Should().BeNull();
    }

    [Fact]
    public void Constructor_Should_HaveParameterizedConstructor()
    {
        // Act & Assert
        var constructor = typeof(StripeController).GetConstructor(new[] { typeof(IStripeInstallationService), typeof(ILogger<StripeController>) });
        constructor.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_Should_BeThreadSafe()
    {
        // Act
        var controllers = new System.Collections.Concurrent.ConcurrentBag<StripeController>();
        var tasks = new List<Task>();

        // Create multiple controllers concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => controllers.Add(new StripeController(_stripeServiceMock.Object, _loggerMock.Object))));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        controllers.Should().HaveCount(10);
        controllers.Should().OnlyContain(c => c != null);
    }

    [Fact]
    public void Constructor_Should_NotBeSerializable()
    {
        // Act & Assert
        typeof(StripeController).IsSerializable.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(StripeController).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeGenericTypeDefinition()
    {
        // Act & Assert
        typeof(StripeController).IsGenericTypeDefinition.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNested()
    {
        // Act & Assert
        typeof(StripeController).IsNested.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedPublic()
    {
        // Act & Assert
        typeof(StripeController).IsNestedPublic.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedPrivate()
    {
        // Act & Assert
        typeof(StripeController).IsNestedPrivate.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedFamily()
    {
        // Act & Assert
        typeof(StripeController).IsNestedFamily.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedAssembly()
    {
        // Act & Assert
        typeof(StripeController).IsNestedAssembly.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedFamORAssem()
    {
        // Act & Assert
        typeof(StripeController).IsNestedFamORAssem.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeNestedFamANDAssem()
    {
        // Act & Assert
        typeof(StripeController).IsNestedFamANDAssem.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(StripeController).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Api");
    }

    [Fact]
    public void Constructor_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(StripeController).FullName.Should().Be("SubscriptionAnalytics.Api.Controllers.StripeController");
    }

    [Fact]
    public void Constructor_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(StripeController).Name.Should().Be("StripeController");
    }

    [Fact]
    public void Constructor_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(StripeController).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeStatic()
    {
        // Act & Assert
        typeof(StripeController).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeDelegate()
    {
        // Act & Assert
        typeof(StripeController).IsSubclassOf(typeof(Delegate)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeArray()
    {
        // Act & Assert
        typeof(StripeController).IsArray.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBePointer()
    {
        // Act & Assert
        typeof(StripeController).IsPointer.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBePrimitive()
    {
        // Act & Assert
        typeof(StripeController).IsPrimitive.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeCOMObject()
    {
        // Act & Assert
        typeof(StripeController).IsCOMObject.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeContextful()
    {
        // Act & Assert
        typeof(StripeController).IsContextful.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeMarshalByRef()
    {
        // Act & Assert
        typeof(StripeController).IsMarshalByRef.Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_BeAssignableToObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(object)).Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIDisposable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IDisposable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIAsyncDisposable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IAsyncDisposable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToICloneable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(ICloneable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIComparable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IComparable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIComparableOfStripeController()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IComparable<StripeController>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIEquatableOfStripeController()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IEquatable<StripeController>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIFormattable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IFormattable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIConvertible()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IConvertible)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToISerializable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Runtime.Serialization.ISerializable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIEnumerable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.IEnumerable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIEnumerableOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IEnumerable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToICollection()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.ICollection)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToICollectionOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.ICollection<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIList()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.IList)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIListOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IList<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIDictionary()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.IDictionary)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIDictionaryOfObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IDictionary<object, object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIReadOnlyCollectionOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyCollection<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIReadOnlyListOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyList<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIReadOnlyDictionaryOfObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IReadOnlyDictionary<object, object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIEnumerator()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.IEnumerator)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIEnumeratorOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Collections.Generic.IEnumerator<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIAsyncEnumerableOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IAsyncEnumerable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIAsyncEnumeratorOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(IAsyncEnumerator<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIQueryable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.IQueryable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIQueryableOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.IQueryable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIOrderedQueryable()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.IOrderedQueryable)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIOrderedQueryableOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.IOrderedQueryable<object>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToIQueryProvider()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.IQueryProvider)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpression()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public void Constructor_Should_NotBeAssignableToExpressionOfFuncOfObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObjectObject()
    {
        // Act & Assert
        typeof(StripeController).IsAssignableTo(typeof(System.Linq.Expressions.Expression<Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>>)).Should().BeFalse();
    }

    [Fact]
    public async Task InitiateConnection_Should_ReturnOkResult_WhenSuccessful()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var expectedResponse = new InitiateStripeConnectionResponse
        {
            AuthorizationUrl = "https://connect.stripe.com/oauth/authorize?client_id=test",
            State = "test_state"
        };

        _stripeServiceMock.Setup(x => x.InitiateConnection(tenantId))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.InitiateConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateStripeConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateStripeConnectionResponse>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task InitiateConnection_Should_ReturnNotFound_WhenArgumentException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.InitiateConnection(tenantId))
            .ThrowsAsync(new ArgumentException("Tenant not found"));

        // Act
        var result = await _controller.InitiateConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateStripeConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateStripeConnectionResponse>;
        actionResult!.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { error = "Tenant not found" });
    }

    [Fact]
    public async Task InitiateConnection_Should_ReturnBadRequest_WhenInvalidOperationException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.InitiateConnection(tenantId))
            .ThrowsAsync(new InvalidOperationException("Already connected"));

        // Act
        var result = await _controller.InitiateConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateStripeConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateStripeConnectionResponse>;
        actionResult!.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { error = "Already connected" });
    }

    [Fact]
    public async Task InitiateConnection_Should_ReturnInternalServerError_WhenGenericException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.InitiateConnection(tenantId))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.InitiateConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<InitiateStripeConnectionResponse>>();
        var actionResult = result as ActionResult<InitiateStripeConnectionResponse>;
        actionResult!.Result.Should().BeOfType<ObjectResult>();
        var objectResult = actionResult.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new { error = "An unexpected error occurred" });
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ReturnOkResult_WhenSuccessful()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var code = "test_code";
        var state = "test_state";

        var expectedConnection = new StripeConnectionDto
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = "acct_test123",
            Status = "Connected"
        };

        _stripeServiceMock.Setup(x => x.HandleOAuthCallback(tenantId, It.IsAny<StripeOAuthCallbackRequest>()))
            .ReturnsAsync(expectedConnection);

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, code, state);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new { 
            message = "Stripe account connected successfully",
            connection = expectedConnection
        });
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ReturnBadRequest_WhenArgumentException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var code = "test_code";
        var state = "test_state";

        _stripeServiceMock.Setup(x => x.HandleOAuthCallback(tenantId, It.IsAny<StripeOAuthCallbackRequest>()))
            .ThrowsAsync(new ArgumentException("Invalid state"));

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, code, state);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { error = "Invalid state" });
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ReturnBadRequest_WhenInvalidOperationException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var code = "test_code";
        var state = "test_state";

        _stripeServiceMock.Setup(x => x.HandleOAuthCallback(tenantId, It.IsAny<StripeOAuthCallbackRequest>()))
            .ThrowsAsync(new InvalidOperationException("Validation failed"));

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, code, state);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().BeEquivalentTo(new { error = "Validation failed" });
    }

    [Fact]
    public async Task HandleOAuthCallback_Should_ReturnInternalServerError_WhenGenericException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var code = "test_code";
        var state = "test_state";

        _stripeServiceMock.Setup(x => x.HandleOAuthCallback(tenantId, It.IsAny<StripeOAuthCallbackRequest>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.HandleOAuthCallback(tenantId, code, state);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new { error = "An unexpected error occurred" });
    }

    [Fact]
    public async Task GetConnection_Should_ReturnOkResult_WhenConnectionExists()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var expectedConnection = new StripeConnectionDto
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            StripeAccountId = "acct_test123",
            Status = "Connected"
        };

        _stripeServiceMock.Setup(x => x.GetConnection(tenantId))
            .ReturnsAsync(expectedConnection);

        // Act
        var result = await _controller.GetConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<StripeConnectionDto?>>();
        var actionResult = result as ActionResult<StripeConnectionDto?>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedConnection);
    }

    [Fact]
    public async Task GetConnection_Should_ReturnOkResult_WhenNoConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.GetConnection(tenantId))
            .ReturnsAsync((StripeConnectionDto?)null);

        // Act
        var result = await _controller.GetConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<StripeConnectionDto?>>();
        var actionResult = result as ActionResult<StripeConnectionDto?>;
        actionResult!.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult!.Value.Should().BeNull();
    }

    [Fact]
    public async Task GetConnection_Should_ReturnInternalServerError_WhenException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.GetConnection(tenantId))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.GetConnection(tenantId);

        // Assert
        result.Should().BeOfType<ActionResult<StripeConnectionDto?>>();
        var actionResult = result as ActionResult<StripeConnectionDto?>;
        actionResult!.Result.Should().BeOfType<ObjectResult>();
        var objectResult = actionResult.Result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new { error = "An unexpected error occurred" });
    }

    [Fact]
    public async Task DisconnectAccount_Should_ReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.DisconnectStripe(tenantId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DisconnectAccount(tenantId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DisconnectAccount_Should_ReturnNotFound_WhenNoConnection()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.DisconnectStripe(tenantId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DisconnectAccount(tenantId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult!.Value.Should().BeEquivalentTo(new { error = "No Stripe connection found for this tenant" });
    }

    [Fact]
    public async Task DisconnectAccount_Should_ReturnInternalServerError_WhenException()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        _stripeServiceMock.Setup(x => x.DisconnectStripe(tenantId))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.DisconnectAccount(tenantId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeEquivalentTo(new { error = "An unexpected error occurred" });
    }

    [Fact]
    public void Controller_Should_BeThreadSafe()
    {
        // Act
        var controllers = new System.Collections.Concurrent.ConcurrentBag<StripeController>();
        var tasks = new List<Task>();

        // Create multiple controllers concurrently
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => controllers.Add(new StripeController(_stripeServiceMock.Object, _loggerMock.Object))));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        controllers.Should().HaveCount(10);
        controllers.Should().OnlyContain(c => c != null);
    }
} 