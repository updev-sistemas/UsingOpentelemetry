using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class OrderItemModelConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        _ = builder.ToTable("tb_orders_items")
            .HasKey(p => p.Id).HasName("id");

        _ = builder.Property(p => p.OrderId).HasColumnName("order_id").IsRequired();
        _ = builder.Property(p => p.ProductId).HasColumnName("product_id").IsRequired();

        _ = builder.Property(p => p.Quantity).HasColumnName("quantity").IsRequired();
        _ = builder.Property(p => p.Cost).HasColumnName("cost").IsRequired();

        _ = builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        _ = builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");


        _ = builder
            .HasOne(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId)
            .IsRequired();

        _ = builder
            .HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .IsRequired();
    }
}
