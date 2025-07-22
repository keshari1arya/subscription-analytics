using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class SyncedCustomerConfiguration : IEntityTypeConfiguration<SyncedCustomer>
{
    public void Configure(EntityTypeBuilder<SyncedCustomer> builder)
    {
        builder.HasKey(sc => sc.CustomerId);
        builder.Property(sc => sc.CustomerId).IsRequired();
        builder.Property(sc => sc.UserId).IsRequired();
        builder.Property(sc => sc.SyncedAt).IsRequired();
        builder.Property(sc => sc.TenantId).IsRequired();
        builder.Property(sc => sc.CreatedAt).IsRequired();
        builder.Property(sc => sc.UpdatedAt).IsRequired();
        builder.Property(sc => sc.IsDeleted).IsRequired();
        builder.Property(sc => sc.DeletedAt).IsRequired(false);
        builder.Property(sc => sc.CreatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(sc => sc.UpdatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(sc => sc.DeletedBy).IsRequired(false).HasMaxLength(450);

        builder.HasOne(sc => sc.Tenant)
            .WithMany(t => t.SyncedCustomers)
            .HasForeignKey(sc => sc.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 