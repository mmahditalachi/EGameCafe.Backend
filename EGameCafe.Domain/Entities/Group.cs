using EGameCafe.Domain.Common;
using EGameCafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Domain.Entities
{
    public class Group : AuditableEntity
    { 
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        public string SharingLink { get; set; }

        public List<GroupMember> GroupMembers { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}
