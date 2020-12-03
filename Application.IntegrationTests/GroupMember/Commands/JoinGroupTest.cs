using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GroupMembers.Commands.JoinGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.GroupMembers.Commands
{
    using static Testing;

    public class JoinGroupTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new JoinGroupCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldReturnDuplicateUserException()
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

            var command = new JoinGroupCommand
            {
                UserId = userId,
                GroupId = groupId
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<DuplicateUserException>();
        }

        [Test]
        public async Task ShouldJoinToGroupAndReturnSucceeded()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = Guid.NewGuid().ToString();

            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink
            };

            await AddAsync(item);

            var command = new JoinGroupCommand
            {
                UserId = userId,
                GroupId = groupId
            };

            var result = await SendAsync(command);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Status.Should().Equals(201);
        }

        [Test]
        public async Task ShouldJoinMemberToGroup()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = Guid.NewGuid().ToString();

            var sharingLink = await GenerateSHA1Hash();

            var item = new Group
            {
                GroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink
            };

            await AddAsync(item);

            var command = new JoinGroupCommand
            {
                UserId = userId,
                GroupId = groupId
            };

            var result = await SendAsync(command);

            var gamingGroup = await FindAsync<GroupMember>(result.Id);

            gamingGroup.Should().NotBeNull();
            gamingGroup.GroupId.Should().Be(groupId);
            gamingGroup.UserId.Should().Be(userId);
            //gamingGroup..Should().Be(userId);
            //gamingGroup.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
