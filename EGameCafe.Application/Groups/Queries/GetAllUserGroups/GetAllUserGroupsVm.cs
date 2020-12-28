using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Groups.Queries.GetAllUserGroups
{
    public class GetAllUserGroupsVm
    {
        public GetAllUserGroupsVm()
        {
            List = new List<GetAllUserGroupsDto>();
        }   

        public IList<GetAllUserGroupsDto> List { get; set; }
    }
}
