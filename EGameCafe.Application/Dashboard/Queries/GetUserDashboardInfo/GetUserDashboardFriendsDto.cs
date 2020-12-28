using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;


namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardFriendsDto 
    {
        public string FriendId { get; set; }
        public string FriendUsername { get; set; }
    }
}
