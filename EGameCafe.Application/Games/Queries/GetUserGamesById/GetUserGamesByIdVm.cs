using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Games.Queries.GetUserGamesById
{
    public class GetUserGamesByIdVm
    {
        public GetUserGamesByIdVm()
        {
            List = new List<GetUserGamesByIdDto>();
        }

        public IList<GetUserGamesByIdDto> List { get; set; }
        public string UserId { get; set; }
    }
}
