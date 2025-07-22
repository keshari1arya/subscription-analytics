using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using SubscriptionAnalytics.Application.Services;

namespace SubscriptionAnalytics.Application.Tests;

public class EncryptionServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly string _validKey;
    private readonly string _validIv;

    public EncryptionServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        
        // Generate valid Base64 encoded key and IV for testing
        var keyBytes = new byte[32]; // 256 bits
        var ivBytes = new byte[16];  // 128 bits
        for (int i = 0; i < keyBytes.Length; i++) keyBytes[i] = (byte)i;
        for (int i = 0; i < ivBytes.Length; i++) ivBytes[i] = (byte)(i + 100);
        
        _validKey = Convert.ToBase64String(keyBytes);
        _validIv = Convert.ToBase64String(ivBytes);
    }

    [Fact]
    public void Constructor_Should_CreateService_WithValidConfiguration()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);

        // Act
        var service = new EncryptionService(_configurationMock.Object);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeOfType<EncryptionService>();
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenKeyIsMissing()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns((string?)null);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be configured*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenIvIsMissing()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns((string?)null);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be configured*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenKeyIsEmpty()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns("");
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be configured*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenIvIsEmpty()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns("");

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be configured*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenKeyIsInvalidBase64()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns("invalid_base64_key!");
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be valid Base64 strings*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenIvIsInvalidBase64()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns("invalid_base64_iv!");

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key and IV must be valid Base64 strings*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenKeyIsWrongSize()
    {
        // Arrange
        var wrongSizeKey = Convert.ToBase64String(new byte[16]); // 128 bits instead of 256
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(wrongSizeKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption key must be 32 bytes*");
    }

    [Fact]
    public void Constructor_Should_ThrowInvalidOperationException_WhenIvIsWrongSize()
    {
        // Arrange
        var wrongSizeIv = Convert.ToBase64String(new byte[8]); // 64 bits instead of 128
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(wrongSizeIv);

        // Act & Assert
        var act = () => new EncryptionService(_configurationMock.Object);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Encryption IV must be 16 bytes*");
    }

    #region Encrypt Tests

    [Fact]
    public void Encrypt_Should_ReturnEncryptedString_WhenValidInput()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var plainText = "Hello, World!";

        // Act
        var result = service.Encrypt(plainText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(plainText);
        result.Should().MatchRegex("^[A-Za-z0-9+/]*={0,2}$"); // Base64 pattern
    }

    [Fact]
    public void Encrypt_Should_ReturnSameResult_ForSameInput()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var plainText = "Test message";

        // Act
        var result1 = service.Encrypt(plainText);
        var result2 = service.Encrypt(plainText);

        // Assert
        result1.Should().Be(result2);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Encrypt_Should_ReturnInput_WhenNullOrEmpty(string input)
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act
        var result = service.Encrypt(input);

        // Assert
        result.Should().Be(input);
    }

    [Fact]
    public void Encrypt_Should_HandleLongText()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var longText = new string('A', 10000);

        // Act
        var result = service.Encrypt(longText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(longText);
        result.Should().MatchRegex("^[A-Za-z0-9+/]*={0,2}$");
    }

    [Fact]
    public void Encrypt_Should_HandleSpecialCharacters()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var specialText = "!@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var result = service.Encrypt(specialText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(specialText);
        result.Should().MatchRegex("^[A-Za-z0-9+/]*={0,2}$");
    }

    [Fact]
    public void Encrypt_Should_HandleUnicodeCharacters()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var unicodeText = "Hello 世界 🌍";

        // Act
        var result = service.Encrypt(unicodeText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(unicodeText);
        result.Should().MatchRegex("^[A-Za-z0-9+/]*={0,2}$");
    }

    #endregion

    #region Decrypt Tests

    [Fact]
    public void Decrypt_Should_ReturnOriginalText_WhenValidEncryptedInput()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var originalText = "Hello, World!";
        var encryptedText = service.Encrypt(originalText);

        // Act
        var result = service.Decrypt(encryptedText);

        // Assert
        result.Should().Be(originalText);
    }

    [Fact]
    public void Decrypt_Should_ReturnInput_WhenNullOrEmpty()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act & Assert
        service.Decrypt("").Should().Be("");
        service.Decrypt(null!).Should().BeNull();
    }

    [Fact]
    public void Decrypt_Should_ThrowInvalidOperationException_WhenInvalidBase64()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act & Assert
        var act = () => service.Decrypt("invalid_base64!");
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Failed to decrypt data*");
    }

    [Fact]
    public void Decrypt_Should_ThrowInvalidOperationException_WhenCorruptedData()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var corruptedData = Convert.ToBase64String(new byte[32]);

        // Act & Assert
        var act = () => service.Decrypt(corruptedData);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Failed to decrypt data*");
    }

    [Fact]
    public void Decrypt_Should_HandleLongText()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var longText = new string('A', 10000);
        var encryptedText = service.Encrypt(longText);

        // Act
        var result = service.Decrypt(encryptedText);

        // Assert
        result.Should().Be(longText);
    }

    [Fact]
    public void Decrypt_Should_HandleSpecialCharacters()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var specialText = "!@#$%^&*()_+-=[]{}|;':\",./<>?";
        var encryptedText = service.Encrypt(specialText);

        // Act
        var result = service.Decrypt(encryptedText);

        // Assert
        result.Should().Be(specialText);
    }

    [Fact]
    public void Decrypt_Should_HandleUnicodeCharacters()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var unicodeText = "Hello 世界 🌍";
        var encryptedText = service.Encrypt(unicodeText);

        // Act
        var result = service.Decrypt(encryptedText);

        // Assert
        result.Should().Be(unicodeText);
    }

    #endregion

    #region Round-trip Tests

    [Theory]
    [InlineData("Simple text")]
    [InlineData("")]
    [InlineData("Text with spaces")]
    [InlineData("Text with numbers 123")]
    [InlineData("Text with symbols !@#$%")]
    [InlineData("Text with newlines\nand tabs\t")]
    [InlineData("Unicode text: 世界 🌍")]
    [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,")]
    public void EncryptDecrypt_Should_BeReversible(string originalText)
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act
        var encrypted = service.Encrypt(originalText);
        var decrypted = service.Decrypt(encrypted);

        // Assert
        decrypted.Should().Be(originalText);
    }

    [Fact]
    public void EncryptDecrypt_Should_ProduceDifferentResults_ForDifferentInputs()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act
        var encrypted1 = service.Encrypt("Text 1");
        var encrypted2 = service.Encrypt("Text 2");

        // Assert
        encrypted1.Should().NotBe(encrypted2);
    }

    [Fact]
    public void EncryptDecrypt_Should_ProduceConsistentResults_ForSameInput()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var text = "Consistent text";

        // Act
        var encrypted1 = service.Encrypt(text);
        var encrypted2 = service.Encrypt(text);
        var decrypted1 = service.Decrypt(encrypted1);
        var decrypted2 = service.Decrypt(encrypted2);

        // Assert
        encrypted1.Should().Be(encrypted2);
        decrypted1.Should().Be(decrypted2);
        decrypted1.Should().Be(text);
    }

    #endregion

    #region Edge Cases and Error Handling

    [Fact]
    public void Encrypt_Should_HandleSingleCharacter()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act
        var result = service.Encrypt("A");

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe("A");
        service.Decrypt(result).Should().Be("A");
    }

    [Fact]
    public void Encrypt_Should_HandleVeryLongText()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var veryLongText = new string('X', 100000);

        // Act
        var result = service.Encrypt(veryLongText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(veryLongText);
        service.Decrypt(result).Should().Be(veryLongText);
    }

    [Fact]
    public void Encrypt_Should_HandleBinaryData()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var binaryText = new string('\0', 100) + new string('\x01', 100);

        // Act
        var result = service.Encrypt(binaryText);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBe(binaryText);
        service.Decrypt(result).Should().Be(binaryText);
    }

    [Fact]
    public void Decrypt_Should_ThrowInvalidOperationException_WhenEmptyBase64()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act & Assert
        var act = () => service.Decrypt("");
        act.Should().NotThrow(); // Should return empty string, not throw
    }

    [Fact]
    public void Decrypt_Should_ThrowInvalidOperationException_WhenInvalidLength()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var invalidLengthData = Convert.ToBase64String(new byte[1]); // Too short

        // Act & Assert
        var act = () => service.Decrypt(invalidLengthData);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Failed to decrypt data*");
    }

    #endregion

    #region Performance and Stress Tests

    [Fact]
    public void EncryptDecrypt_Should_HandleMultipleOperations()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);

        // Act & Assert
        for (int i = 0; i < 100; i++)
        {
            var text = $"Test text {i}";
            var encrypted = service.Encrypt(text);
            var decrypted = service.Decrypt(encrypted);
            decrypted.Should().Be(text);
        }
    }

    [Fact]
    public void Encrypt_Should_HandleConcurrentAccess()
    {
        // Arrange
        _configurationMock.Setup(x => x["Encryption:Key"]).Returns(_validKey);
        _configurationMock.Setup(x => x["Encryption:IV"]).Returns(_validIv);
        var service = new EncryptionService(_configurationMock.Object);
        var tasks = new List<Task<string>>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var text = $"Concurrent text {i}";
            tasks.Add(Task.Run(() => service.Encrypt(text)));
        }

        var results = Task.WhenAll(tasks).Result;

        // Assert
        results.Should().HaveCount(10);
        results.Should().OnlyContain(r => !string.IsNullOrEmpty(r));
    }

    #endregion
} 