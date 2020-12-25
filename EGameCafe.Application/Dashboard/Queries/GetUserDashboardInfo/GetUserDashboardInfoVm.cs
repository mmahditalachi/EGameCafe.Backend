using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardInfoVm
    {
        public GetUserDashboardInfoVm()
        {
            GameList = new List<GetUserDashboardGameDto>();
            ActivityList = new List<GetUserDashboardActivityDto>();
            SystemInfo = new GetUserDashboardSystemDto();
            PersonalInfo = new GetUserDashboardPersonalDto();
        }

        public IList<GetUserDashboardGameDto> GameList { get; set; }
        public IList<GetUserDashboardActivityDto> ActivityList { get; set; }
        public GetUserDashboardSystemDto SystemInfo { get; set; }
        public GetUserDashboardPersonalDto PersonalInfo { get; set; }
        public string UserId { get; set; }
    }


}
