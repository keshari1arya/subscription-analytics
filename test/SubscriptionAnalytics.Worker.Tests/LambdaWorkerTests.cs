using Amazon.Lambda.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Worker;

namespace SubscriptionAnalytics.Worker.Tests;

public class LambdaWorkerTests
{
    private readonly Mock<ILogger<LambdaWorker>> _loggerMock;
    private readonly Mock<ILambdaContext> _contextMock;
    private readonly LambdaWorker _worker;

    public LambdaWorkerTests()
    {
        _loggerMock = new Mock<ILogger<LambdaWorker>>();
        _contextMock = new Mock<ILambdaContext>();
        _worker = new LambdaWorker(_loggerMock.Object);

        // Setup default context mock
        _contextMock.Setup(c => c.FunctionName).Returns("test-function");
        _contextMock.Setup(c => c.FunctionVersion).Returns("test-version");
        _contextMock.Setup(c => c.AwsRequestId).Returns("test-request-id");
    }

    [Fact]
    public void LambdaWorker_Should_BeInstantiable()
    {
        // Act & Assert
        _worker.Should().NotBeNull();
    }

    [Fact]
    public void LambdaWorker_Should_ImplementILambdaWorker()
    {
        // Act & Assert
        _worker.Should().BeAssignableTo<ILambdaWorker>();
    }

    [Fact]
    public async Task ExecuteAsync_Should_ReturnStream()
    {
        // Arrange
        var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test data"));

        // Act
        var result = await _worker.ExecuteAsync(inputStream, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Stream>();
    }

    [Fact]
    public async Task ExecuteAsync_Should_ReturnEmptyStream()
    {
        // Arrange
        var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test data"));

        // Act
        var result = await _worker.ExecuteAsync(inputStream, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleNullInputStream()
    {
        // Act
        var result = await _worker.ExecuteAsync(null!, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleEmptyInputStream()
    {
        // Arrange
        var inputStream = new MemoryStream();

        // Act
        var result = await _worker.ExecuteAsync(inputStream, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleLargeInputStream()
    {
        // Arrange
        var largeData = new string('x', 10000);
        var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(largeData));

        // Act
        var result = await _worker.ExecuteAsync(inputStream, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleNullContext()
    {
        // Arrange
        var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test data"));

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => 
            _worker.ExecuteAsync(inputStream, null!));
    }

    [Fact]
    public void LambdaWorker_Should_BeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var workers = new System.Collections.Concurrent.ConcurrentBag<LambdaWorker>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => workers.Add(new LambdaWorker(_loggerMock.Object))));
        }

        // Assert
        Task.WhenAll(tasks).Wait();
        workers.Should().HaveCount(10);
        workers.Should().OnlyContain(w => w != null);
    }

    [Fact]
    public void LambdaWorker_Should_BeReusable()
    {
        // Arrange
        var worker1 = new LambdaWorker(_loggerMock.Object);
        var worker2 = new LambdaWorker(_loggerMock.Object);

        // Act & Assert
        worker1.Should().NotBeSameAs(worker2);
        worker1.Should().NotBeNull();
        worker2.Should().NotBeNull();
    }

    [Fact]
    public void LambdaWorker_Should_HaveCorrectType()
    {
        // Act & Assert
        _worker.Should().BeOfType<LambdaWorker>();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeAbstract()
    {
        // Act & Assert
        typeof(LambdaWorker).IsAbstract.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_BePublic()
    {
        // Act & Assert
        typeof(LambdaWorker).IsPublic.Should().BeTrue();
    }

    [Fact]
    public void LambdaWorker_Should_BeClass()
    {
        // Act & Assert
        typeof(LambdaWorker).IsClass.Should().BeTrue();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeInterface()
    {
        // Act & Assert
        typeof(LambdaWorker).IsInterface.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeEnum()
    {
        // Act & Assert
        typeof(LambdaWorker).IsEnum.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeValueType()
    {
        // Act & Assert
        typeof(LambdaWorker).IsValueType.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeSealed()
    {
        // Act & Assert
        typeof(LambdaWorker).IsSealed.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_NotBeGeneric()
    {
        // Act & Assert
        typeof(LambdaWorker).IsGenericType.Should().BeFalse();
    }

    [Fact]
    public void LambdaWorker_Should_HaveCorrectNamespace()
    {
        // Act & Assert
        typeof(LambdaWorker).Namespace.Should().Be("SubscriptionAnalytics.Worker");
    }

    [Fact]
    public void LambdaWorker_Should_HaveCorrectAssembly()
    {
        // Act & Assert
        typeof(LambdaWorker).Assembly.GetName().Name.Should().Be("SubscriptionAnalytics.Worker");
    }

    [Fact]
    public void LambdaWorker_Should_HaveCorrectFullName()
    {
        // Act & Assert
        typeof(LambdaWorker).FullName.Should().Be("SubscriptionAnalytics.Worker.LambdaWorker");
    }

    [Fact]
    public void LambdaWorker_Should_HaveCorrectName()
    {
        // Act & Assert
        typeof(LambdaWorker).Name.Should().Be("LambdaWorker");
    }

    [Fact]
    public void LambdaWorker_Should_HaveExecuteAsyncMethod()
    {
        // Act & Assert
        typeof(LambdaWorker).GetMethod("ExecuteAsync").Should().NotBeNull();
    }

    [Fact]
    public void LambdaWorker_Should_HaveConstructor()
    {
        // Act & Assert
        typeof(LambdaWorker).GetConstructor(new[] { typeof(ILogger<LambdaWorker>) }).Should().NotBeNull();
    }

    [Fact]
    public void LambdaWorker_Should_HandleNullLogger()
    {
        // Act & Assert
        Action act = () => new LambdaWorker(null!);
        act.Should().NotThrow();
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleDisposedStream()
    {
        // Arrange
        var inputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test data"));
        inputStream.Dispose();

        // Act
        var result = await _worker.ExecuteAsync(inputStream, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleUnreadableStream()
    {
        // Arrange
        var inputStream = new Mock<Stream>();
        inputStream.Setup(s => s.CanRead).Returns(false);

        // Act
        var result = await _worker.ExecuteAsync(inputStream.Object, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_HandleStreamWithException()
    {
        // Arrange
        var inputStream = new Mock<Stream>();
        inputStream.Setup(s => s.CanRead).Returns(true);
        inputStream.Setup(s => s.Length).Throws(new IOException("Test stream exception"));

        // Act
        var result = await _worker.ExecuteAsync(inputStream.Object, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }
} 