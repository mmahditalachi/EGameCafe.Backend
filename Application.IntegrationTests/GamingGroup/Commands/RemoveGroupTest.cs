using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GamingGroup.Commands.CreateGroup;
using EGameCafe.Application.GamingGroup.Commands.Removegroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.GamingGroup.Commands
{
    using static Testing;

    public class RemoveGroupTest : TestBase
    {
        [Test]
        public void ShouldRequireValid64CharGroupId()
        {
            var command = new RemoveGroupCommand { GroupId = "9" };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidGrouptId()
        {
            var randomId = await GenerateRandomId();

            var command = new RemoveGroupCommand{ GroupId = randomId };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRemoveGroupList()
        {
            var command = new CreateGroupCommand { GroupName = "gptest", GroupType = GroupType.privateGroup };

            var result = await SendAsync(command);

            await SendAsync(new RemoveGroupCommand { GroupId = result.Id });
          
            var list = await FindAsync<GamingGroups>(result.Id);

            list.Should().BeNull();
        }
    }
}
