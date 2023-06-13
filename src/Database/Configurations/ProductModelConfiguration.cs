using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations;

public class ProductModelConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("tb_products")
            .HasKey(p => p.Id).HasName("id");

        builder.Property(p => p.Code).HasColumnName("code").IsRequired();
        builder.Property(p => p.Description).HasColumnName("description").IsRequired();
        builder.Property(p => p.Price).HasColumnName("price").IsRequired();
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
    }
}
