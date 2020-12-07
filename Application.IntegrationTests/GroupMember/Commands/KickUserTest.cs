using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GroupMembers.Commands.KickUser;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace Application.IntegrationTests.GroupMembers.Commands
{
    using static Testing;

    public class KickUserTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new KickUserCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldKickFromGroupAndReturnSucceeded()
        {

            var userId = await RunAsDefaultUserAsync();

            var groupId = Guid.NewGuid().ToString();

            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink,
            };

            await AddAsync(item);

            var groupMemberId = Guid.NewGuid().ToString();

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                GroupMemberId = groupMemberId,
                UserId = userId
            };

            await AddAsync(groupMember);

            var command = new KickUserCommand
            {
                GroupId = groupId,
                UserId = userId
            };

            var result = await SendAsync(command);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Status.Should().Equals(200);

        }

        [Test]
        public async Task ShouldKickFromGroup()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = Guid.NewGuid().ToString();

            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink,
            };

            await AddAsync(item);

            var groupMemberId = Guid.NewGuid().ToString();

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                GroupMemberId = groupMemberId,
                UserId = userId
            };

            await AddAsync(groupMember);

            var command = new KickUserCommand
            {
                GroupId = groupId,
                UserId = userId
            };

            var result = await SendAsync(command);

            var gamingGroup = await FindAsync<GroupMember>(result.Id);

            gamingGroup.Should().NotBeNull();
            gamingGroup.GroupId.Should().Be(groupId);
            gamingGroup.UserId.Should().Be(userId);
            gamingGroup.IsBlock.Should().Be(true);
            //gamingGroup..Should().Be(userId);
            //gamingGroup.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }

        [Test]
        public void ShouldNotFindUser()
        {
            var groupId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var command = new KickUserCommand
            {
                GroupId = groupId,
                UserId = userId
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

    }
}
