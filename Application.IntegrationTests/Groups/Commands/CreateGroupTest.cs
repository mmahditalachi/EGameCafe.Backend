using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Groups.Commands.CreateGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Groups.Commands
{
    using static Testing;

    public class CreateGroupTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateGroupCommand { GroupName = "", GroupType = GroupType.publicGroup };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateGroupAndReturnSucceeded()
        {
            var game = new Game { GameId = Guid.NewGuid().ToString(), GameName = "test" };

            await AddAsync(game);

            var command = new CreateGroupCommand
            {
                GroupName = "gptest",
                GroupType = GroupType.privateGroup,
                GameId = game.GameId,
            };

            var result = await SendAsync(command);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Status.Should().Equals(201);
        }

        [Test]
        public async Task ShouldCreateGroup()
        {
            var userId = await RunAsDefaultUserAsync();
            var gameId = Guid.NewGuid().ToString();

            var game = new Game { GameId = gameId, GameName = "test" };
            await AddAsync(game);

            var command = new CreateGroupCommand { 
                GroupName = "gptest", 
                GroupType = GroupType.privateGroup,
                GameId = game.GameId,
            };

            var result = await SendAsync(command);

            var gamingGroup = await FindAsync<Group>(result.Id);

            gamingGroup.Should().NotBeNull();
            gamingGroup.GroupName.Should().Be(command.GroupName);
            gamingGroup.GroupType.Should().Be(command.GroupType);

            gamingGroup.GameId.Should().Be(gameId);

            gamingGroup.GameId.Should().Be(gameId);

            gamingGroup.CreatedBy.Should().Be(userId);

            gamingGroup.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }

    }
}
