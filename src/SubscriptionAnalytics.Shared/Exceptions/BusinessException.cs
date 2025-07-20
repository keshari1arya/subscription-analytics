namespace SubscriptionAnalytics.Shared.Exceptions;

public class BusinessException : Exception
{
    public string ErrorCode { get; }

    public BusinessException(string message, string errorCode = "BUSINESS_ERROR") 
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public BusinessException(string message, Exception innerException, string errorCode = "BUSINESS_ERROR") 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}

public class NotFoundException : BusinessException
{
    public NotFoundException(string message) 
        : base(message, "NOT_FOUND")
    {
    }

    public NotFoundException(string entityName, object id) 
        : base($"{entityName} with ID {id} was not found", "NOT_FOUND")
    {
    }
}

public class ValidationException : BusinessException
{
    public Dictionary<string, string[]> ValidationErrors { get; }

    public ValidationException(string message, Dictionary<string, string[]> validationErrors) 
        : base(message, "VALIDATION_ERROR")
    {
        ValidationErrors = validationErrors;
    }
}

public class UnauthorizedException : BusinessException
{
    public UnauthorizedException(string message = "Unauthorized access") 
        : base(message, "UNAUTHORIZED")
    {
    }
}

public class ForbiddenException : BusinessException
{
    public ForbiddenException(string message = "Access forbidden") 
        : base(message, "FORBIDDEN")
    {
    }
} 