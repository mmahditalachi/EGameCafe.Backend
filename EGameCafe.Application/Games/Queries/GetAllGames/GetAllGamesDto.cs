using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;

namespace EGameCafe.Application.Games.Queries.GetAllGames
{
    public class GetAllGamesDto : IMapFrom<GetAllGamesDto>
    {
        public string GameName { get; set; }
        public string GameId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Game, GetAllGamesDto>();
        }
    }
}
