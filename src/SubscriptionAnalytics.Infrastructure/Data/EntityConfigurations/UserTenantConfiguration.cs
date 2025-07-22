using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class UserTenantConfiguration : IEntityTypeConfiguration<UserTenant>
{
    public void Configure(EntityTypeBuilder<UserTenant> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.TenantId });
        builder.Property(ut => ut.UserId).IsRequired();
        builder.Property(ut => ut.Role).IsRequired().HasMaxLength(100);
        builder.Property(ut => ut.TenantId).IsRequired();
        builder.Property(ut => ut.CreatedAt).IsRequired();
        builder.Property(ut => ut.UpdatedAt).IsRequired();
        builder.Property(ut => ut.IsDeleted).IsRequired();
        builder.Property(ut => ut.DeletedAt).IsRequired(false);
        builder.Property(ut => ut.CreatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(ut => ut.UpdatedBy).IsRequired(false).HasMaxLength(450);
        builder.Property(ut => ut.DeletedBy).IsRequired(false).HasMaxLength(450);

        builder.HasOne(ut => ut.Tenant)
            .WithMany(t => t.UserTenants)
            .HasForeignKey(ut => ut.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 