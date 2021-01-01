using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Groups.Commands.CreateGroup;
using EGameCafe.Application.Groups.Commands.Removegroup;
using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Application.Groups.Queries.GetGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
namespace Application.IntegrationTests.Groups.Queries
{
    using static Testing;

    public class GetGroupByIdTest : TestBase
    {
        [Test]
        public async Task ShouldReturnGroupById()
        {
            var groupId = Guid.NewGuid().ToString();
            var gameId = Guid.NewGuid().ToString();
            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink,
                Game = new Game { GameName = "test", GameId = gameId}
            };

            await AddAsync(item);

            var query = new GetGroupByIdQuery(groupId);

            var result = await SendAsync(query);

            result.GroupInfo.GroupName.Should().Equals(item.GroupName);
            result.GroupInfo.GroupType.Should().Equals(item.GroupType);
            result.GroupInfo.GameName.Should().Equals(item.GroupType);
            
        }

        [Test]
        public async Task ShouldGroupGameBeNull()
        {
            var groupId = Guid.NewGuid().ToString();
            var gameId = Guid.NewGuid().ToString();
            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink,
                Game = new Game { GameId = gameId, GameName = "test"}
            };

            await AddAsync(item);

            var query = new GetGroupByIdQuery(groupId);

            var result = await SendAsync(query);

            result.GroupInfo.GroupName.Should().Equals(item.GroupName);
            result.GroupInfo.GroupType.Should().Equals(item.GroupType);

            result.GroupInfo.GameName.Should().Equals(item.Game.GameName);
        }
    }
}
