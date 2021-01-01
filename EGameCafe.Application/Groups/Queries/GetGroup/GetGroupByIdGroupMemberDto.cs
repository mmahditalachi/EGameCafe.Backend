using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using System.Collections.Generic;


namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdGroupMemberDto : IMapFrom<GetGroupByIdGroupMemberDto>
    {
        public bool IsBlock { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupMember, GetGroupByIdGroupMemberDto>()
                .ForMember(e => e.Username, e => e.MapFrom(e => e.UserDetail.Username))
                .ForMember(e => e.Username, e => e.MapFrom(e => e.UserDetail.Username));
        }
    }
}
