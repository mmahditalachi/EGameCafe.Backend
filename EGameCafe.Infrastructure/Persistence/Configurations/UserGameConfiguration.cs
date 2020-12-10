using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder.HasKey(e => e.UserGameId);

            builder.Property(e => e.UserGameId)
                .HasMaxLength(64);

            builder.Property(e => e.UserId)
                .HasMaxLength(64);

            builder.Property(e => e.GameId)
                .HasMaxLength(64);
        }
    }
}
