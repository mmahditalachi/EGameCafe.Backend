using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.GamingGroup.Queries.GetAllGroups
{
    public class GetAllGroupsDto : IMapFrom<GetAllGroupsDto>
    {
        public string GamingGroupGroupId { get; set; }
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GamingGroups, GetAllGroupsDto>();
        }
    }
}
