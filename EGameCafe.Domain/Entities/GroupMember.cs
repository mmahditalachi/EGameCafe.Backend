using EGameCafe.Domain.Common;

namespace EGameCafe.Domain.Entities
{
    public class GroupMember : AuditableEntity
    {
        public string GroupMemberId { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public Group Group { get; set; }
        public bool IsBlock { get; set; }
    }
}
