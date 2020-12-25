using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using EGameCafe.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {

        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser user) => await userManager.CreateAsync(user, "password");

        public static async Task SeedSampleDataAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {

            var user_1 = new ApplicationUser { Id = "fc7b1548-cb36-43f1-8c96-4c5843629a68", Email = "test1@test.com", FirstName = "mohammad", LastName = "Talachi", UserName = "User_Test_1", PhoneNumber = "09354891892", BirthDate = new DateTime(1999, 11, 24), EmailConfirmed =true };
            var user_2 = new ApplicationUser { Id = "cc011c60-fc7f-4dd2-906e-f89b23796831", Email = "test2@test.com", FirstName = "arshia", LastName = "majidi", UserName = "User_Test_2", PhoneNumber = "09354891892", BirthDate = new DateTime(1999, 11, 24), EmailConfirmed =true};

            await SeedDefaultUserAsync(userManager, user_1);

            await SeedDefaultUserAsync(userManager, user_2);

            var gameId_1 = Guid.NewGuid().ToString();
            var gameId_2 = Guid.NewGuid().ToString();

            var game1 = new Game
            {
                GameId = gameId_1,
                GameName = "test__1",
            };

            var game2 = new Game
            {
                GameId = gameId_2,
                GameName = "test__2",
            };

            // Seed, if necessary
            if (!context.UserDetails.Any())
            {
                var items = new List<UserDetail>
                {
                    new UserDetail
                    {
                        UserId = user_1.Id,
                        Username = user_1.UserName,
                        Fullname = user_1.FirstName,
                        UserGames = new List<UserGame>()
                        {
                            new UserGame { UserId = user_1.Id, Game = game1 , UserGameId = Guid.NewGuid().ToString() },
                            new UserGame { UserId = user_1.Id, Game = game2 , UserGameId = Guid.NewGuid().ToString() }
                        }
                    },
                    new UserDetail
                    {
                        UserId = user_2.Id,
                        Username = user_2.UserName,
                        Fullname = user_2.FirstName,
                        UserGames = new List<UserGame>()
                        {
                            new UserGame { UserId = user_1.Id, Game = game1 , UserGameId = Guid.NewGuid().ToString() },
                            new UserGame { UserId = user_1.Id, Game = game2 , UserGameId = Guid.NewGuid().ToString() }
                        }
                    },
                };

                await context.UserDetails.AddRangeAsync(items);

                await context.SaveChangesAsync();
            }

            if (!context.Activity.Any())
            {
                var item1 = new Activity
                {
                    UserId = user_1.Id,
                    ActivityId = Guid.NewGuid().ToString(),
                    ActivityTitle = "morning",
                    ActivityText = "Hi whats up guys...",
                };

                context.Activity.Add(item1);

                await context.SaveChangesAsync();
            }

            if (!context.UserSystemInfo.Any())
            {
                var item1 = new UserSystemInfo
                {
                    UserSystemInfoId = Guid.NewGuid().ToString(),
                    CaseManufacturer = SystemManufacturer.NZXT,
                    CaseName = "NZXT",
                    UserId = user_1.Id,
                    CpuManufacturer = CpuManufacturer.Amd,
                    CpuName = "3600xt",
                    GraphicCardManufacturer = SystemManufacturer.Asus,
                    GraphicCardName = "rtx 3070",
                    PowerManufacturer = SystemManufacturer.CoolerMaster,
                    PowerName = "600w silver",
                    RamManufacturer = SystemManufacturer.Corsair,
                    TotalRam = 16,
                };

                context.UserSystemInfo.Add(item1);

                await context.SaveChangesAsync();
            }

            if (!context.GameGenres.Any())
            {

                var genreId = Guid.NewGuid().ToString();

                var item1 = new Genre
                {
                    Id = genreId,
                    Title = "action adventure",
                };

                var gameGenres = new GameGenre { GameGenreId = Guid.NewGuid().ToString(), Game = game1, Genre = item1 };

                context.GameGenres.Add(gameGenres);

                await context.SaveChangesAsync();

            }
            if (!context.Group.Any())
            {
                var groups = new List<Group>
                {

                   new Group
                   {
                        GroupId = Guid.NewGuid().ToString(),
                        GroupName = "gpTest_1",
                        GroupType = GroupType.publicGroup,
                        SharingLink = "DB4C76CF1EE2B97DF9E180314C74F22384C06E82",
                        GameId = gameId_1
                   },
                   new Group
                   {
                        GroupId = Guid.NewGuid().ToString(),
                        GroupName = "gpTest_2",
                        GroupType = GroupType.publicGroup,
                        SharingLink = "DB4C76CF1EE2B97DF9E180314C74F55684C06E82",
                        GameId = gameId_2
                   },

                };

                await context.Group.AddRangeAsync(groups);

                await context.SaveChangesAsync();
            }

            if (!context.GroupMember.Any())
            {
                var items = new List<GroupMember>
                {
                 new GroupMember
                {
                    GroupMemberId = Guid.NewGuid().ToString(),
                    IsBlock = false,
                    GroupId = Guid.NewGuid().ToString(),
                    UserId =  user_1.Id
                },

                new GroupMember
                {
                    GroupMemberId = Guid.NewGuid().ToString(),
                    IsBlock = false,
                    GroupId = Guid.NewGuid().ToString(),
                    UserId = user_2.Id
                }
            };

                await context.GroupMember.AddRangeAsync(items);

                await context.SaveChangesAsync();
            }

        }
    }
}
