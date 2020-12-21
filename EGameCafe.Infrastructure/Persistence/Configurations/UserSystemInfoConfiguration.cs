using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class UserSystemInfoConfiguration : IEntityTypeConfiguration<UserSystemInfo>
    {
        public void Configure(EntityTypeBuilder<UserSystemInfo> builder)
        {
            builder.HasKey(e => e.UserSystemInfoId);


            builder.Property(e => e.UserSystemInfoId)
                .HasMaxLength(64);

            builder.Property(e => e.CaseName)
                .HasMaxLength(50);

            builder.Property(e => e.GraphicCardName)
                .HasMaxLength(50);

            builder.Property(e => e.CpuName)
                .HasMaxLength(50);

            builder.Property(e => e.PowerName)
                .HasMaxLength(50);
        }
    }
}
