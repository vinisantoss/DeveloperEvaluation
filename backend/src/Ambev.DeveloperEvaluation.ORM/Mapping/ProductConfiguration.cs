using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for CommercialProduct entity mapping
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(cp => cp.Id);
        builder.Property(cp => cp.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(cp => cp.ExternalId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cp => cp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cp => cp.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cp => cp.StandardPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(cp => cp.ExternalId)
            .IsUnique()
            .HasDatabaseName("IX_Products_ExternalId");
    }
}