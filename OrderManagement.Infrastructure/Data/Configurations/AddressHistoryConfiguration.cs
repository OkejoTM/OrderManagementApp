using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Data.Configurations;

public class AddressHistoryConfiguration : IEntityTypeConfiguration<AddressHistory>
{
    public void Configure(EntityTypeBuilder<AddressHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.PumpingDate)
            .IsRequired();

        builder.Property(h => h.CubeAmount)
            .IsRequired();

        builder.Property(h => h.PaymentType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(h => h.Price)
            .IsRequired();

        builder.HasIndex(h => h.AddressId);
    }
}