using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Games.Queries.GetAllGames
{
    public class GetAllGamesVm
    {
        public GetAllGamesVm()
        {
            List = new List<GetAllGamesDto>();
        }

        public IList<GetAllGamesDto> List { get; set; }
        public int TotalGames { get; set; }
    }
}
