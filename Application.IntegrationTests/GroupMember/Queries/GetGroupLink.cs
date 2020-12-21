using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Groups.Queries.SendInvitation;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace Application.IntegrationTests.GroupMembers.Queries
{
    using static Testing;

    public class GetGroupLink : TestBase
    {
        [Test]
        public void ShouldRequireNotNullFields()
        {
            var command = new SendInvitationQuery();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldGenerateInvitationToken()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = Guid.NewGuid().ToString();

            var result = new SendInvitationQuery
            {
                GroupId = groupId,
            };

            result.Should().NotBeNull();
        }

    }
}
