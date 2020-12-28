using System.Collections.Generic;


namespace EGameCafe.Application.Groups.Queries.GetAllGroups
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
