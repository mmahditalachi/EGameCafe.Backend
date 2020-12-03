using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GroupMembers.Commands.JoinViaInvitation;
using EGameCafe.Application.GroupMembers.Queries.SendInvitation;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.IntegrationTests.GroupMembers.Commands
{
    using static Testing;

    public class JoinViaInvitationTest : TestBase
    {
        public void ShouldRequireNotNullFields()
        {
            var command = new JoinViaInvitationCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldReturnNotFoundException()
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

            Random rand = new Random();

            int randomNumber = rand.Next(1, 50);

            var randomString = GenerateRandomString(randomNumber);

            var joiningCommand = new JoinViaInvitationCommand { token = randomString, UserId = userId };

            FluentActions.Invoking(() =>
                SendAsync(joiningCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldJoinGroupViaInvitation()
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

            var sendingCommand = new SendInvitationQuery
            {
                GroupId = groupId,
                UserId = userId
            };

            var sendingResult = await SendAsync(sendingCommand);

            var joiningCommand = new JoinViaInvitationCommand { token = sendingResult, UserId = userId };

            var joiningResult = await SendAsync(joiningCommand);

            var gamingGroup = await FindAsync<GroupMember>(joiningResult.Id);

            gamingGroup.Should().NotBeNull();

            gamingGroup.GroupId.Should().Be(groupId);
            gamingGroup.UserId.Should().Be(userId);
        }
    }
}
