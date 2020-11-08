using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.GamingGroup.Queries.GetAllGroups
{
    public class GetAllGroupsVm
    {
        public GetAllGroupsVm()
        {
            List = new List<GetAllGroupsDto>();
        }

        public IList<GetAllGroupsDto> List { get; set; }
        public int TotalGroups { get; set; }
    }
}
