using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(255);
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.Property(t => t.IsDeleted).IsRequired();
        builder.Property(t => t.DeletedAt).IsRequired(false);
        builder.Property(t => t.CreatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(t => t.UpdatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(t => t.DeletedBy).IsRequired(false).HasMaxLength(450);
    }
} 