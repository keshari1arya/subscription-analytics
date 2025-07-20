namespace SubscriptionAnalytics.Shared.DTOs;

using System.ComponentModel.DataAnnotations;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTenantRequest
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;
}

public class UserTenantDto
{
    public string UserId { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? TenantName { get; set; }
    public string? UserEmail { get; set; }
}

public class AssignUserToTenantRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class UserTenantsResponse
{
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<UserTenantDto> Tenants { get; set; } = new List<UserTenantDto>();
} 