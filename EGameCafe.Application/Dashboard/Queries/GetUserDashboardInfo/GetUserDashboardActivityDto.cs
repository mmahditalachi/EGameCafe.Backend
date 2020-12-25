using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using System;

namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardActivityDto : IMapFrom<GetUserDashboardActivityDto>
    {
        public string ActivityId { get; set; }
        public string ActivityTitle { get; set; }
        public string ActivityText { get; set; }
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Activity, GetUserDashboardActivityDto>();
        }
    }
}
