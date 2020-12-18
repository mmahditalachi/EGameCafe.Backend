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
        public async Task ShouldJoinUserToMultipleGroup()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId_1 = Guid.NewGuid().ToString();
            var groupId_2 = Guid.NewGuid().ToString();

            var sharingLink = await GenerateSHA1Hash();

            var gp1 = new Group
            {
                GroupId = groupId_1,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink
            };

            var gp2 = new Group
            {
                GroupId = groupId_2,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = sharingLink
            };

            await AddAsync(gp1);
            await AddAsync(gp2);

            var command_1 = new JoinGroupCommand
            {
                UserId = userId,
                GroupId = groupId_1
            };

            var command_2 = new JoinGroupCommand
            {
                UserId = userId,
                GroupId = groupId_2
            };

            var result_1 = await SendAsync(command_1);

            result_1.Should().NotBeNull();
            result_1.Succeeded.Should().BeTrue();
            result_1.Status.Should().Equals(201);

            var result_2 = await SendAsync(command_2);

            result_2.Should().NotBeNull();
            result_2.Succeeded.Should().BeTrue();
            result_2.Status.Should().Equals(201);
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
