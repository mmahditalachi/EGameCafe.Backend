using EGameCafe.Domain.Common;

namespace EGameCafe.Domain.Entities
{
    public class Activity : AuditableEntity
    {
        public string ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityText { get; set; }
        public string UserId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
