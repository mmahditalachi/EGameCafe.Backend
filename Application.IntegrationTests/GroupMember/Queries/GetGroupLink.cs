using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GroupMember.Queries.SendInvitation;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;


namespace Application.IntegrationTests.GroupMember.Queries
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

            var groupId = await GenerateRandomId();

            var result = new SendInvitationQuery
            {
                GroupId = groupId,
                UserId = userId
            };

            result.Should().NotBeNull();
        }

    }
}
