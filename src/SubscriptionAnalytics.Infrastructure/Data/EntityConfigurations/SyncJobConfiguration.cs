using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class SyncJobConfiguration : IEntityTypeConfiguration<SyncJob>
{
    public void Configure(EntityTypeBuilder<SyncJob> builder)
    {
        builder.ToTable("SyncJobs");

        builder.HasKey(sj => sj.Id);

        builder.Property(sj => sj.JobType)
            .IsRequired();

        builder.Property(sj => sj.Status)
            .IsRequired();

        builder.Property(sj => sj.Progress)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(sj => sj.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(sj => sj.StartedAt);

        builder.Property(sj => sj.CompletedAt);

        builder.Property(sj => sj.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(sj => sj.ProviderName)
            .HasMaxLength(50);

        builder.HasIndex(sj => sj.TenantId);
        builder.HasIndex(sj => sj.Status);
        builder.HasIndex(sj => sj.StartedAt);
    }
}
