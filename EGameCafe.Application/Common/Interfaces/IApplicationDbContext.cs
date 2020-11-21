using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<OTP> OTP { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<GamingGroupMembers> GroupMembers { get; set; }
        DbSet<GamingGroups> GamingGroups { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
