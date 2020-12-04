using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Groups.Commands.CreateGroup;
using EGameCafe.Application.Groups.Commands.Removegroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Groups.Commands
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
        public void  ShouldRequireValidGrouptId()
        {
            var randomId = Guid.NewGuid().ToString();

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
          
            var list = await FindAsync<Group>(result.Id);

            list.Should().BeNull();
        }
    }
}
