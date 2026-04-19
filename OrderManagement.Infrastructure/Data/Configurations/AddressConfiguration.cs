using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.NormalizedName)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(a => a.AreaId);

        builder.HasIndex(a => new { a.AreaId, a.NormalizedName })
            .IsUnique();

        builder.HasMany(a => a.Histories)
            .WithOne(h => h.Address)
            .HasForeignKey(h => h.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(Address.Histories))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}