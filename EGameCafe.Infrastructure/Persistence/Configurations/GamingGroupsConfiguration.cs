using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class GamingGroupsConfiguration : IEntityTypeConfiguration<GamingGroups>
    {
        public void Configure(EntityTypeBuilder<GamingGroups> builder)
        {
            builder.HasKey(e => e.GamingGroupGroupId);

            builder.Property(e => e.GamingGroupGroupId)
                .HasMaxLength(64);
                
            builder.Property(e => e.GroupName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.GroupType)
                .IsRequired();
        }
    }
}
