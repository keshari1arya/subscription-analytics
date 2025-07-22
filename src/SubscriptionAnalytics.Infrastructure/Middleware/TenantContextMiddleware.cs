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
                
                // Also set it in the HttpContext.Items for easy access throughout the request
                context.Items["TenantId"] = tenantId.Value;
                
                _logger.LogDebug("Tenant context set to: {TenantId}", tenantId.Value);
            }
            else
            {
                // Set a default tenant ID for now (in production, you might want to handle this differently)
                tenantContext.TenantId = Guid.Empty;
                context.Items["TenantId"] = Guid.Empty;
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
        // 1. Custom header: X-Tenant-Id (HIGHEST PRIORITY)
        // 2. URL path: /api/stripe/tenant/{tenantId}/... or /api/tenant/{tenantId}/...
        // 3. Query parameter: tenantId
        // 4. JWT token claim: tenant_id
        // 5. Subdomain (if applicable)

        // 1. Check custom header (HIGHEST PRIORITY)
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            if (Guid.TryParse(headerValue.ToString(), out var tenantId))
            {
                _logger.LogDebug("Tenant ID extracted from header: {TenantId}", tenantId);
                return tenantId;
            }
        }

        // 2. Check URL path for tenant ID
        var path = context.Request.Path.Value;
        if (!string.IsNullOrEmpty(path))
        {
            // Pattern: /api/stripe/tenant/{tenantId}/...
            if (path.StartsWith("/api/stripe/tenant/"))
            {
                var parts = path.Split('/');
                if (parts.Length >= 4 && Guid.TryParse(parts[3], out var tenantId))
                {
                    _logger.LogDebug("Tenant ID extracted from URL path: {TenantId}", tenantId);
                    return tenantId;
                }
            }
            
            // Pattern: /api/tenant/{tenantId}/...
            if (path.StartsWith("/api/tenant/"))
            {
                var parts = path.Split('/');
                if (parts.Length >= 3 && Guid.TryParse(parts[2], out var tenantId))
                {
                    _logger.LogDebug("Tenant ID extracted from URL path: {TenantId}", tenantId);
                    return tenantId;
                }
            }
        }

        // 3. Check query parameter
        if (context.Request.Query.TryGetValue("tenantId", out var queryValue))
        {
            if (Guid.TryParse(queryValue.ToString(), out var tenantId))
            {
                _logger.LogDebug("Tenant ID extracted from query parameter: {TenantId}", tenantId);
                return tenantId;
            }
        }

        // 4. Check JWT token claim
        var user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = user.FindFirst("tenant_id");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                _logger.LogDebug("Tenant ID extracted from JWT claim: {TenantId}", tenantId);
                return tenantId;
            }
        }

        // 5. Check subdomain (for future use)
        // var subdomain = ExtractSubdomain(context.Request.Host.Host);
        // if (!string.IsNullOrEmpty(subdomain))
        // {
        //     // Look up tenant by subdomain
        //     return await GetTenantIdBySubdomain(subdomain);
        // }

        _logger.LogDebug("No tenant ID found in any source");
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