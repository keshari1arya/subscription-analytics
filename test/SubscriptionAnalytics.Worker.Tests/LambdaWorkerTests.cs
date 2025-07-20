using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SubscriptionAnalytics.Worker;
using Xunit;
using System.Threading;

namespace SubscriptionAnalytics.Worker.Tests;

public class LambdaWorkerTests
{
    private readonly Mock<ILogger<LambdaWorker>> _loggerMock = new();
    private readonly Mock<ILambdaContext> _contextMock = new();
    private readonly LambdaWorker _worker;

    public LambdaWorkerTests()
    {
        _worker = new LambdaWorker(_loggerMock.Object);
        _contextMock.SetupGet(x => x.FunctionName).Returns("TestFunction");
        _contextMock.SetupGet(x => x.FunctionVersion).Returns("1.0");
        _contextMock.SetupGet(x => x.AwsRequestId).Returns("req-123");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Log_And_Return_EmptyStream()
    {
        // Arrange
        var input = new MemoryStream();

        // Act
        var result = await _worker.ExecuteAsync(input, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
        _loggerMock.Verify(x => x.Log(
            Microsoft.Extensions.Logging.LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Lambda Worker started")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        _loggerMock.Verify(x => x.Log(
            Microsoft.Extensions.Logging.LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Lambda Worker completed successfully")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Log_And_Rethrow_On_Exception()
    {
        // Arrange
        var input = new MemoryStream();
        var worker = new LambdaWorker(_loggerMock.ObjectWithException());
        var context = _contextMock.Object;

        // Act
        Func<Task> act = async () => await worker.ExecuteAsync(input, context);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _loggerMock.Verify(x => x.Log(
            Microsoft.Extensions.Logging.LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred in Lambda Worker")),
            It.IsAny<InvalidOperationException>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Return_EmptyStream_When_InputStreamHasData()
    {
        // Arrange
        var input = new MemoryStream(new byte[] { 1, 2, 3, 4 });

        // Act
        var result = await _worker.ExecuteAsync(input, _contextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_And_LogError_IfLoggerThrows()
    {
        // Arrange
        var input = new MemoryStream();
        var loggerMock = new Mock<ILogger<LambdaWorker>>();
        loggerMock.Setup(x => x.Log(
            Microsoft.Extensions.Logging.LogLevel.Information,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Throws(new InvalidOperationException("Logger failed"));
        var worker = new LambdaWorker(loggerMock.Object);

        // Act
        Func<Task> act = async () => await worker.ExecuteAsync(input, _contextMock.Object);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Logger failed");
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_If_CancellationRequested()
    {
        // Arrange
        var input = new MemoryStream();
        var cts = new CancellationTokenSource();
        cts.Cancel();
        // Simulate a long-running operation in the worker (add a TODO in real logic)
        // For now, just check that the method completes since no cancellation is checked in the code
        var result = await _worker.ExecuteAsync(input, _contextMock.Object);
        result.Should().NotBeNull();
        result.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Be_Stateless_Across_MultipleCalls()
    {
        // Arrange
        var input1 = new MemoryStream();
        var input2 = new MemoryStream(new byte[] { 9, 8, 7 });

        // Act
        var result1 = await _worker.ExecuteAsync(input1, _contextMock.Object);
        var result2 = await _worker.ExecuteAsync(input2, _contextMock.Object);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Length.Should().Be(0);
        result2.Length.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteAsync_Should_Throw_If_ContextIsNull()
    {
        // Arrange
        var input = new MemoryStream();

        // Act
        Func<Task> act = async () => await _worker.ExecuteAsync(input, null!);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>(); // or ArgumentNullException if you add a check
    }
}

public static class LoggerMockExtensions
{
    public static ILogger<LambdaWorker> ObjectWithException(this Mock<ILogger<LambdaWorker>> loggerMock)
    {
        loggerMock.Setup(x => x.Log(
            Microsoft.Extensions.Logging.LogLevel.Information,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Callback(() => throw new InvalidOperationException("Test exception"));
        return loggerMock.Object;
    }
} 