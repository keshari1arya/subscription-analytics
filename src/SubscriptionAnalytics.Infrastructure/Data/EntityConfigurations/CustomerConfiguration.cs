using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Name)
            .HasMaxLength(255);

        builder.Property(c => c.Email)
            .HasMaxLength(255);

        builder.Property(c => c.Phone)
            .HasMaxLength(50);

        builder.Property(c => c.CustomerCreatedAt)
            .IsRequired();

        builder.Property(c => c.Livemode)
            .IsRequired();

        builder.Property(c => c.SyncedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => new { c.TenantId, c.CustomerId })
            .IsUnique();

        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.Email);
        builder.HasIndex(c => c.SyncedAt);

        // Relationships
        builder.HasMany(c => c.StripeCustomers)
            .WithOne(sc => sc.Customer)
            .HasForeignKey(sc => sc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
