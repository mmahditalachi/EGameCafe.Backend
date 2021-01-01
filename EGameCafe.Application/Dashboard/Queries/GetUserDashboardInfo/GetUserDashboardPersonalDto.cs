using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using System;

namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardPersonalDto : IMapFrom<GetUserDashboardPersonalDto>
    {
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public DateTime Created { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDetail, GetUserDashboardPersonalDto>();
        }
    }
}
