using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using System.Collections.Generic;

namespace EGameCafe.Application.UsersDetails.Queries.GettUserDetail
{
    public class GetUserGameDto : IMapFrom<GetUserGameDto>
    {
        public string GameId { get; set; }
        public string GameName { get; set; }
    }
}
