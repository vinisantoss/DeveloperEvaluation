using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

/// <summary>
/// Configuration for TransactionItem entity mapping
/// </summary>
public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
{
    public void Configure(EntityTypeBuilder<TransactionItem> builder)
    {
        builder.ToTable("TransactionItems");

        builder.HasKey(ti => ti.Id);
        builder.Property(ti => ti.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(ti => ti.Quantity)
            .IsRequired();

        builder.Property(ti => ti.ItemPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ti => ti.DiscountPercentage)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(ti => ti.ItemTotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ti => ti.IsCancelled)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ti => ti.ProductId)
            .IsRequired();

        builder.Property(ti => ti.TransactionId)
            .IsRequired();

        builder.HasOne(ti => ti.Product)
            .WithMany()
            .HasForeignKey(ti => ti.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ti => ti.ProductId)
            .HasDatabaseName("IX_TransactionItems_ProductId");

        builder.HasIndex(ti => ti.TransactionId)
            .HasDatabaseName("IX_TransactionItems_TransactionId");
    }
}