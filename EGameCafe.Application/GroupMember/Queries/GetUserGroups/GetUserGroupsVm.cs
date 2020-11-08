using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.GroupMember.Queries.GetUserGroups
{
    public class GetUserGroupsVm
    {
        public GetUserGroupsVm()
        {
            List = new List<GetUserGroupsDto>();
        }

        public IList<GetUserGroupsDto> List { get; set; }

    }
}
