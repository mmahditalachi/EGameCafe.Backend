using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using System.Collections.Generic;


namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdInfoDto : IMapFrom<GetGroupByIdInfoDto>
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        public string GameName { get; set; }
        public string SharingLink { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GetGroupByIdInfoDto>()
                .ForMember(e=>e.GameName, e=>e.MapFrom(e=>e.Game.GameName));
        }
    }
}
