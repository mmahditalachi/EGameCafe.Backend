using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;

namespace EGameCafe.Application.Games.Queries.GetUserGamesById
{
    public class GetUserGamesByIdDto : IMapFrom<GetUserGamesByIdDto>
    {
        public string GameId { get; set; }
        public string GameName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserGame, GetUserGamesByIdDto>()
                .ForMember(e=>e.GameId, e=>e.MapFrom(p=>p.Game.GameId))
                .ForMember(e=>e.GameName, e=>e.MapFrom(p=>p.Game.GameName));
        }
    }
}
