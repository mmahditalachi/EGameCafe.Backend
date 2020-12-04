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
    public class GroupMembersConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.ToTable("GroupMember");

            builder.HasKey(e => e.GroupMemberId);

            builder.Property(e => e.GroupMemberId)
                .HasMaxLength(64);

            builder.Property(e => e.UserId)
                .IsRequired();
            
        }
    }
}
