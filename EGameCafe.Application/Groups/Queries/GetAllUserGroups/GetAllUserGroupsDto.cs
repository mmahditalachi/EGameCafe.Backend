using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;

namespace EGameCafe.Application.Groups.Queries.GetAllUserGroups
{
    public class GetAllUserGroupsDto : IMapFrom<GetAllUserGroupsDto>
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        public string GameName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GetAllUserGroupsDto>()
                .ForMember(e => e.GameName, e => e.MapFrom(e => e.Game.GameName));

        }
    }
}
