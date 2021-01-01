using EGameCafe.Domain.Common;
using EGameCafe.Domain.Enums;


namespace EGameCafe.Domain.Entities
{
    public class UserSystemInfo : AuditableEntity
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

        public string UserId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
