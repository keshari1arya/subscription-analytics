using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class StripeConnectionConfiguration : IEntityTypeConfiguration<StripeConnection>
{
    public void Configure(EntityTypeBuilder<StripeConnection> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TenantId).IsUnique();
        builder.HasIndex(e => e.StripeAccountId).IsUnique();

        builder.Property(e => e.AccessToken).IsRequired();
        builder.Property(e => e.RefreshToken).IsRequired(false);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.StripeAccountId).IsRequired();
        builder.Property(e => e.ConnectedAt).IsRequired();
        builder.Property(e => e.TenantId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
        builder.Property(e => e.IsDeleted).IsRequired();
        builder.Property(e => e.DeletedAt).IsRequired(false);
        builder.Property(e => e.CreatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(e => e.UpdatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(e => e.DeletedBy).IsRequired(false).HasMaxLength(450);

        builder.HasOne(e => e.Tenant)
            .WithOne(t => t.StripeConnection)
            .HasForeignKey<StripeConnection>(e => e.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 