using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class ActivityVote : AuditableEntity
    {
        public string ActivityVoteId { get; set; }
        public Activity Activity { get; set; }
        public string ActivityId { get; set; }
        public UserDetail UserDetail { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }
    }
}
