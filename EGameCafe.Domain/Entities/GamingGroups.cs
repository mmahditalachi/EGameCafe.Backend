using EGameCafe.Domain.Common;
using EGameCafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Domain.Entities
{
    public class GamingGroups : AuditableEntity
    { 
        public string GamingGroupGroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        
        public List<GamingGroupMembers> GroupMembers { get; set; }
    }
}
