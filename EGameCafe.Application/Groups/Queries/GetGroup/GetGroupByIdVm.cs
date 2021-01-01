using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdVm
    {
        public GetGroupByIdVm()
        {
            GroupMembers = new List<GetGroupByIdGroupMemberDto>();
            GroupInfo = new GetGroupByIdInfoDto();
        }
        public IList<GetGroupByIdGroupMemberDto> GroupMembers { get; set; }
        public GetGroupByIdInfoDto GroupInfo { get; set; }
    }
}
