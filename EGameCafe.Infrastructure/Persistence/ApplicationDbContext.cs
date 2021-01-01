using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Domain.Common;
using EGameCafe.Domain.Entities;
using EGameCafe.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Persistence
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser>,
        IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<OTP> OTP { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<GroupMember> GroupMember { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserSystemInfo> UserSystemInfo { get; set; }
        public DbSet<FriendRequest> FriendRequest { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<ActivityVote> ActivityVotes { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<GroupMember>()
                        .HasOne<Group>(e => e.Group)
                        .WithMany(d => d.GroupMembers)
                        .HasForeignKey(e => e.GroupId);

            //many to many game and genre 

            builder.Entity<GameGenre>()
                        .HasOne<Genre>(e => e.Genre)
                        .WithMany(p => p.GameGenres)
                        .HasForeignKey(e => e.GenreId);

            builder.Entity<GameGenre>()
                        .HasOne<Game>(e => e.Game)
                        .WithMany(p => p.GameGenres)
                        .HasForeignKey(e => e.GameId);

            //many to many game and userDetail

            builder.Entity<UserGame>()
                        .HasOne<UserDetail>(e => e.UserDetail)
                        .WithMany(p => p.UserGames)
                        .HasForeignKey(e => e.UserId);

            builder.Entity<UserGame>()
                        .HasOne<Game>(e => e.Game)
                        .WithMany(p => p.UserGames)
                        .HasForeignKey(e => e.GameId);

            // one to many group and game

            builder.Entity<Group>()
                        .HasOne<Game>(e => e.Game)
                        .WithMany(d => d.Groups)
                        .HasForeignKey(e => e.GameId);

            // one to many GroupMember and UserDetail

            builder.Entity<GroupMember>()
                        .HasOne<UserDetail>(e => e.UserDetail)
                        .WithMany(d => d.GroupMembers)
                        .HasForeignKey(e => e.UserId);

            // one to many Activity and UserDetail

            builder.Entity<Activity>()
                        .HasOne<UserDetail>(e => e.UserDetail)
                        .WithMany(d => d.Activities)
                        .HasForeignKey(e => e.UserId);

            // one to one UserSystemInfo and UserDetail

            builder.Entity<UserDetail>()
                        .HasOne<UserSystemInfo>(p => p.UserSystemInfo)
                        .WithOne(s => s.UserDetail)
                        .HasForeignKey<UserSystemInfo>(e=>e.UserId);

            //many to many Activity and userDetail

            builder.Entity<ActivityVote>()
                        .HasOne<UserDetail>(e => e.UserDetail)
                        .WithMany(p => p.ActivityVotes);

            builder.Entity<ActivityVote>()
                        .HasOne<Activity>(e => e.Activity)
                        .WithMany(p => p.ActivityVotes);

            base.OnModelCreating(builder);
        }

    }


}
