using Microsoft.EntityFrameworkCore;
using SubscriptionAnalytics.Shared.Entities;
using SubscriptionAnalytics.Shared.Interfaces;

namespace SubscriptionAnalytics.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<UserTenant> UserTenants { get; set; } = null!;
    public DbSet<SyncedCustomer> SyncedCustomers { get; set; } = null!;

    private readonly Guid? _tenantId;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantId = tenantContext?.TenantId;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tenant
        modelBuilder.Entity<Tenant>().HasKey(t => t.Id);
        modelBuilder.Entity<Tenant>().Property(t => t.Name).IsRequired().HasMaxLength(255);
        modelBuilder.Entity<Tenant>().Property(t => t.CreatedAt).IsRequired();

        // UserTenant
        modelBuilder.Entity<UserTenant>().HasKey(ut => new { ut.UserId, ut.TenantId });
        modelBuilder.Entity<UserTenant>().Property(ut => ut.Role).IsRequired().HasMaxLength(100);

        // SyncedCustomer
        modelBuilder.ApplyConfiguration<SyncedCustomer>(new Configuration.SyncedCustomerConfiguration());

        // Global query filter for tenant_id
        if (_tenantId.HasValue)
        {
            modelBuilder.Entity<SyncedCustomer>().HasQueryFilter(c => c.TenantId == _tenantId.Value);
        }
    }
} 