
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for BusinessPartner entity mapping
/// </summary>
public class BusinessPartnerConfiguration : IEntityTypeConfiguration<BusinessPartner>
{
    public void Configure(EntityTypeBuilder<BusinessPartner> builder)
    {
        builder.ToTable("BusinessPartners");

        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(bp => bp.ExternalId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bp => bp.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bp => bp.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bp => bp.Document)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(bp => bp.ExternalId)
            .IsUnique()
            .HasDatabaseName("IX_BusinessPartners_ExternalId");
    }
}