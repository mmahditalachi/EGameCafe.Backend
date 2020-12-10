using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class GroupsConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Group");

            builder.HasKey(e => e.GroupId);

            builder.Property(e => e.GroupId)
                .HasMaxLength(64);
                
            builder.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.GroupType)
                .IsRequired();

            builder.Property(e => e.SharingLink)
                .IsRequired();
        }
    }
}
