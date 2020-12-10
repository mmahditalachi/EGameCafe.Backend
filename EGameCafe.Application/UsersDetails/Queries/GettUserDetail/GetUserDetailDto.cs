using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using System.Collections.Generic;


namespace EGameCafe.Application.UsersDetails.Queries.GettUserDetail
{
    public class GetUserDetailDto : IMapFrom<GetUserDetailDto>
    {
        public string UserId { get; set; }

        public ICollection<GetUserGameDto> UserGames { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDetail, GetUserDetailDto>();

            profile.CreateMap<UserGame, GetUserGameDto>()
                .ForMember(e => e.GameId, e => e.MapFrom(e => e.GameId))
                .ForMember(e => e.GameName, e => e.MapFrom(e => e.Game.GameName));
        }
    }
}