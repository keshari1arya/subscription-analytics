using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Configuration;

public class SyncedCustomerConfiguration : IEntityTypeConfiguration<SyncedCustomer>
{
    public void Configure(EntityTypeBuilder<SyncedCustomer> builder)
    {
        builder.HasKey(c => c.CustomerId);
        builder.Property(c => c.CustomerId).HasMaxLength(255);
        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.Name).HasMaxLength(255);
        builder.Property(c => c.Email).HasMaxLength(255);
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.CreatedStripeAt).IsRequired();
        builder.Property(c => c.Livemode).IsRequired();
        // TODO: Write tests for Metadata property (JsonObject) when provider support/value converter is added
        // builder.Property(c => c.Metadata).HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
        builder.Property(c => c.SyncedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.TenantId);
    }
} 