using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionAnalytics.Shared.Entities;

namespace SubscriptionAnalytics.Infrastructure.Data.EntityConfigurations;

public class ProviderConnectionConfiguration : IEntityTypeConfiguration<ProviderConnection>
{
    public void Configure(EntityTypeBuilder<ProviderConnection> builder)
    {
        builder.ToTable("ProviderConnections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProviderName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ProviderAccountId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.AccessToken)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(4000);

        builder.Property(x => x.TokenType)
            .HasMaxLength(50);

        builder.Property(x => x.Scope)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.AdditionalData)
            .HasColumnType("jsonb")
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null)
            );

        // Indexes
        builder.HasIndex(x => new { x.TenantId, x.ProviderName })
            .IsUnique();

        builder.HasIndex(x => x.ProviderAccountId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ConnectedAt);
    }
} 