using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for CommercialTransaction entity mapping
/// </summary>
public class CommercialTransactionConfiguration : IEntityTypeConfiguration<CommercialTransaction>
{
    public void Configure(EntityTypeBuilder<CommercialTransaction> builder)
    {
        builder.ToTable("CommercialTransactions");

        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(ct => ct.TransactionCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ct => ct.TransactionDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ct => ct.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ct => ct.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(ct => ct.BusinessPartnerId)
            .IsRequired();

        builder.Property(ct => ct.OperationalUnitId)
            .IsRequired();

        builder.HasOne(ct => ct.BusinessPartner)
            .WithMany()
            .HasForeignKey(ct => ct.BusinessPartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ct => ct.OperationalUnit)
            .WithMany()
            .HasForeignKey(ct => ct.OperationalUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ct => ct.Items)
            .WithOne()
            .HasForeignKey(ti => ti.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ct => ct.TransactionCode)
            .IsUnique()
            .HasDatabaseName("IX_CommercialTransactions_TransactionCode");

        builder.HasIndex(ct => ct.TransactionDate)
            .HasDatabaseName("IX_CommercialTransactions_TransactionDate");

        builder.HasIndex(ct => ct.BusinessPartnerId)
            .HasDatabaseName("IX_CommercialTransactions_BusinessPartnerId");

        builder.HasIndex(ct => ct.OperationalUnitId)
            .HasDatabaseName("IX_CommercialTransactions_OperationalUnitId");
    }
}