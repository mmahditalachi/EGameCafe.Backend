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
        }

        public IList<GetUserDashboardGameDto> GameList { get; set; }
        public string UserId { get; set; }
    }
}
