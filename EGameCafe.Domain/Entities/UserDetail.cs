using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class UserDetail : AuditableEntity
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }

        public UserSystemInfo UserSystemInfo { get; set; }

        public List<GroupMember> GroupMembers { get; set; }
        public List<Activity> Activities { get; set; }
        public ICollection<UserGame> UserGames  { get; set; }
    }
}
