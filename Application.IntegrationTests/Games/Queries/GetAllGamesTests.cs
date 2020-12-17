using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Games.Queries.GetAllGames;
using EGameCafe.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Games.Queries
{
    using static Testing;

    public class GetAllGamesTests : TestBase
    {

        //[Test]
        //public void ShouldThrowNotFound()
        //{
        //    var query = new GetAllGamesQuery(0, 10, "");

        //    FluentActions.Invoking(() =>
        //        SendAsync(query)).Should().Throw<NotFoundException>();
        //}

        [Test]
        public async Task ShouldReturnAllGames()
        {
            await AddAsync(new Game
            {
                GameId = Guid.NewGuid().ToString(),
                GameName = "gpTest",
            });

            var query = new GetAllGamesQuery(0, 10, "");

            var result = await SendAsync(query);

            result.List.Should().HaveCount(1);
            result.TotalGames.Should().Equals(1);
        }

        
    }
}
