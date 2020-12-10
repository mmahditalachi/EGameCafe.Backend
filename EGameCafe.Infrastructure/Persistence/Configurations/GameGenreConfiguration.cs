using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasKey(e => e.GameGenreId);

            builder.Property(e => e.GameGenreId)
                .HasMaxLength(64);

            builder.Property(e => e.GameId)
                .HasMaxLength(64);

            builder.Property(e => e.GenreId)
                .HasMaxLength(64);
        }
    }
}
