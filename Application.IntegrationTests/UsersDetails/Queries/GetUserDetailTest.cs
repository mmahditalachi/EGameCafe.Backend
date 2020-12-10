using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Application.UsersDetails.Queries.GettUserDetail;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IntegrationTests.UsersDetails.Queries
{
    using static Testing;

    public class GetUserDetailTest : TestBase
    {
        [Test]
        public async Task ShouldReturnUserDetail()
        {
            var userId = await RunAsDefaultUserAsync();

            var game1 = new Game { GameName = "test_1", GameId = Guid.NewGuid().ToString() };
            var game2 = new Game { GameName = "test_2", GameId = Guid.NewGuid().ToString() };

            await AddAsync(new UserDetail
            {
                UserId = userId,
                UserGames = new List<UserGame>()
                { 
                    new UserGame { UserId = userId, Game = game1 , UserGameId = Guid.NewGuid().ToString() },
                    new UserGame { UserId = userId, Game = game2 , UserGameId = Guid.NewGuid().ToString() }
                }
            });

            var query = new GetUserDetailQuery(userId);

            var result = await SendAsync(query);

            result.UserId.Should().Equals(userId);

            result.UserGames.Should().HaveCount(2);
            
        }
    }
}
