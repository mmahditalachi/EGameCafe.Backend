using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EGameCafe.Infrastructure.Persistence.Configurations
{
    public class ActivityVoteConfiguration : IEntityTypeConfiguration<ActivityVote>
    {
        public void Configure(EntityTypeBuilder<ActivityVote> builder)
        {
            builder.HasKey(e => new { e.ActivityId, e.UserId, e.ActivityVoteId });
        }
    }
}
