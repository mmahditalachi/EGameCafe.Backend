using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;


namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardGameDto : IMapFrom<GetUserDashboardGameDto>
    {
        public string GameId { get; set; }
        public string GameName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserGame, GetUserDashboardGameDto>()
                .ForMember(e => e.GameId, e => e.MapFrom(p => p.Game.GameId))
                .ForMember(e => e.GameName, e => e.MapFrom(p => p.Game.GameName));
        }
    }
}
