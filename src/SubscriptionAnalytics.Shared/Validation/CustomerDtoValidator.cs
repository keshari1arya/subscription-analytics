using FluentValidation;

namespace SubscriptionAnalytics.Shared.Validation;

public class CustomerDtoValidator : AbstractValidator<DTOs.CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
} 