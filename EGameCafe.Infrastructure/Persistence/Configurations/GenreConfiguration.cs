using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasMaxLength(64);

            builder.Property(e => e.Title)
                .HasMaxLength(100)
               .IsRequired();
        }
    }
}
