using HealthEquity.Assessment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthEquity.Assessment.Infrastructure.Configurations;
public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Make)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder
            .Property(b => b.Model)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder
            .Property(b => b.Year)
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(b => b.Doors)
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(b => b.Color)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder
            .Property(b => b.Price)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();
    }
}
