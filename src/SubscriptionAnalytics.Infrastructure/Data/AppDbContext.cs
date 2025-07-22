using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<UserTenant> UserTenants { get; set; } = null!;
    public DbSet<SyncedCustomer> SyncedCustomers { get; set; } = null!;
    public DbSet<StripeConnection> StripeConnections { get; set; } = null!;

    private readonly Guid? _tenantId;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, Guid? tenantId) : base(options)
    {
        _tenantId = tenantId;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Apply tenant-specific query filters if tenant context is available
        if (_tenantId.HasValue)
        {
            modelBuilder.Entity<UserTenant>().HasQueryFilter(ut => ut.TenantId == _tenantId.Value);
            modelBuilder.Entity<SyncedCustomer>().HasQueryFilter(c => c.TenantId == _tenantId.Value);
            modelBuilder.Entity<StripeConnection>().HasQueryFilter(sc => sc.TenantId == _tenantId.Value);
        }
    }
} 