using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(e => e.ActivityId);

            builder.Property(e => e.ActivityId)
                .HasMaxLength(64);

            builder.Property(e => e.ActivityTitle)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.ActivityText)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
