using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SubscriptionAnalytics.Shared.Interfaces;
using System.Security.Claims;

namespace SubscriptionAnalytics.Infrastructure.Middleware;

public class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantContextMiddleware> _logger;

    public TenantContextMiddleware(RequestDelegate next, ILogger<TenantContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        try
        {
            // Extract tenant from various sources
            var tenantId = ExtractTenantId(context);
            
            if (tenantId.HasValue)
            {
                tenantContext.TenantId = tenantId.Value;
                _logger.LogDebug("Tenant context set to: {TenantId}", tenantId.Value);
            }
            else
            {
                // Set a default tenant ID for now (in production, you might want to handle this differently)
                tenantContext.TenantId = Guid.Empty;
                _logger.LogDebug("No tenant context found, using default tenant");
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in tenant context middleware");
            throw;
        }
    }

    private Guid? ExtractTenantId(HttpContext context)
    {
        // Priority order for tenant extraction:
        // 1. Custom header: X-Tenant-Id
        // 2. Query parameter: tenantId
        // 3. JWT token claim: tenant_id
        // 4. Subdomain (if applicable)

        // 1. Check custom header
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            if (Guid.TryParse(headerValue.ToString(), out var tenantId))
            {
                return tenantId;
            }
        }

        // 2. Check query parameter
        if (context.Request.Query.TryGetValue("tenantId", out var queryValue))
        {
            if (Guid.TryParse(queryValue.ToString(), out var tenantId))
            {
                return tenantId;
            }
        }

        // 3. Check JWT token claim
        var user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = user.FindFirst("tenant_id");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                return tenantId;
            }
        }

        // 4. Check subdomain (for future use)
        // var subdomain = ExtractSubdomain(context.Request.Host.Host);
        // if (!string.IsNullOrEmpty(subdomain))
        // {
        //     // Look up tenant by subdomain
        //     return await GetTenantIdBySubdomain(subdomain);
        // }

        return null;
    }

    private string? ExtractSubdomain(string host)
    {
        var parts = host.Split('.');
        if (parts.Length > 2)
        {
            return parts[0];
        }
        return null;
    }
} 