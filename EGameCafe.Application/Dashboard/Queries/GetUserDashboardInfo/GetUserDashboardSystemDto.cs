using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;

namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardSystemDto : IMapFrom<GetUserDashboardSystemDto>
    {
        public string UserSystemInfoId { get; set; }

        public SystemManufacturer RamManufacturer { get; set; }
        public int TotalRam { get; set; }
        public SystemManufacturer GraphicCardManufacturer { get; set; }
        public string GraphicCardName { get; set; }
        public CpuManufacturer CpuManufacturer { get; set; }
        public string CpuName { get; set; }
        public SystemManufacturer CaseManufacturer { get; set; }
        public string CaseName { get; set; }
        public SystemManufacturer PowerManufacturer { get; set; }
        public string PowerName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserSystemInfo, GetUserDashboardSystemDto>();
        }
    }
}
