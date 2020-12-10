using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using System.Collections.Generic;


namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdDto : IMapFrom<GetGroupByIdDto>
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        public List<GroupMember> GroupMembers { get; set; }
        public string GameName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GetGroupByIdDto>()
                .ForMember(e=>e.GameName, e=>e.MapFrom(e=>e.Game.GameName));
        }
    }
}
