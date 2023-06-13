using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class OrderModelConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        _ = builder.ToTable("tb_orders")
            .HasKey(p => p.Id).HasName("id");

        _ = builder.Property(p => p.CustomerId).HasColumnName("customer_id").IsRequired();
        _ = builder.Property(p => p.Payment).HasColumnName("id_payment").IsRequired();
        _ = builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        _ = builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");

        _ = builder
            .HasMany(p => p.Items)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .IsRequired();

        _ = builder
            .HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .IsRequired();
    }
}
