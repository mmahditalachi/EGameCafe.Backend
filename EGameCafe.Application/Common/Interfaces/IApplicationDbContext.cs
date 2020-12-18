using EGameCafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<OTP> OTP { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<GroupMember> GroupMember { get; set; }
        DbSet<Group> Group { get; set; }
        DbSet<Game> Game { get; set; }
        DbSet<Genre> Genre { get; set; }
        DbSet<GameGenre> GameGenres { get; set; }
        DbSet<UserDetail> UserDetails { get; set; }
        DbSet<UserGame> UserGames { get; set; }
        DbSet<Activity> Activity { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
