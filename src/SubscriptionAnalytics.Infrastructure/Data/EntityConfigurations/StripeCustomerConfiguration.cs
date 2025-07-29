using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class StripeCustomerConfiguration : IEntityTypeConfiguration<StripeCustomer>
{
    public void Configure(EntityTypeBuilder<StripeCustomer> builder)
    {
        builder.ToTable("StripeCustomers");

        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.CustomerId)
            .IsRequired();

        builder.Property(sc => sc.StripeCustomerId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(sc => sc.DefaultPaymentMethod)
            .HasMaxLength(255);

        builder.Property(sc => sc.TaxInfo)
            .HasMaxLength(255);

        builder.Property(sc => sc.TaxInfoVerification)
            .HasMaxLength(255);

        builder.Property(sc => sc.Delinquent)
            .IsRequired();

        builder.Property(sc => sc.Discount);

        builder.Property(sc => sc.InvoicePrefix)
            .HasMaxLength(255);

        builder.Property(sc => sc.InvoiceSettings)
            .HasMaxLength(1000);

        builder.Property(sc => sc.NextInvoiceSequence)
            .HasMaxLength(255);

        builder.Property(sc => sc.PreferredLocales)
            .HasMaxLength(500);

        builder.Property(sc => sc.Shipping)
            .HasMaxLength(1000);

        builder.Property(sc => sc.Source)
            .HasMaxLength(1000);

        builder.Property(sc => sc.Subscriptions)
            .HasMaxLength(1000);

        builder.Property(sc => sc.Tax)
            .HasMaxLength(1000);

        builder.Property(sc => sc.TaxExempt)
            .HasMaxLength(255);

        builder.Property(sc => sc.TestClock)
            .HasMaxLength(255);

        // Indexes
        builder.HasIndex(sc => new { sc.TenantId, sc.StripeCustomerId })
            .IsUnique();

        builder.HasIndex(sc => sc.CustomerId);
        builder.HasIndex(sc => sc.TenantId);

        // Relationships
        builder.HasOne(sc => sc.Customer)
            .WithMany(c => c.StripeCustomers)
            .HasForeignKey(sc => sc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
