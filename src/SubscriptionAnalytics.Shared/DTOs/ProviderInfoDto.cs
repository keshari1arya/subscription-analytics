namespace SubscriptionAnalytics.Shared.DTOs;

public record ProviderInfoDto(string ProviderName, string DisplayName, bool SupportsOAuth);
public record OAuthUrlResponse(string Url);

public record SuccessResponseDto(bool Success, string Message);

public record ErrorResponseDto(string Error);