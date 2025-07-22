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

        // Configure Tenant (minimal configuration for primary key)
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(t => t.Id);
        });

        // Configure UserTenant (minimal configuration for composite primary key)
        modelBuilder.Entity<UserTenant>(entity =>
        {
            entity.HasKey(ut => new { ut.UserId, ut.TenantId });
        });

        // Configure SyncedCustomer (minimal configuration for primary key)
        modelBuilder.Entity<SyncedCustomer>(entity =>
        {
            entity.HasKey(sc => sc.CustomerId);
        });

        // Configure StripeConnection
        modelBuilder.Entity<StripeConnection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TenantId).IsUnique();
            entity.HasIndex(e => e.StripeAccountId).IsUnique();

            entity.Property(e => e.AccessToken).IsRequired();
            entity.Property(e => e.RefreshToken).IsRequired(false);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.StripeAccountId).IsRequired();

            entity.HasOne(e => e.Tenant)
                .WithOne(t => t.StripeConnection)
                .HasForeignKey<StripeConnection>(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Apply tenant-specific query filters if tenant context is available
        if (_tenantId.HasValue)
        {
            modelBuilder.Entity<UserTenant>().HasQueryFilter(ut => ut.TenantId == _tenantId.Value);
            modelBuilder.Entity<SyncedCustomer>().HasQueryFilter(c => c.TenantId == _tenantId.Value);
            modelBuilder.Entity<StripeConnection>().HasQueryFilter(sc => sc.TenantId == _tenantId.Value);
        }
    }
} 