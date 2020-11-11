using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GamingGroup.Commands.CreateGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.GamingGroup.Commands
{
    using static Testing;

    public class CreateGroupTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateGroupCommand("", GroupType.publicGroup);

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateGamingGroupAndReturnSucceeded()
        {
            var command = new CreateGroupCommand("gptest",GroupType.privateGroup);

            var result = await SendAsync(command);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Status.Should().Equals(201);
        }

        [Test]
        public async Task ShouldCreateGamingGroup()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateGroupCommand("gptest",GroupType.privateGroup);

            var result = await SendAsync(command);

            var gamingGroup = await FindAsync<GamingGroups>(result.Id);

            gamingGroup.Should().NotBeNull();
            gamingGroup.GroupName.Should().Be(command.GroupName);
            gamingGroup.GroupType.Should().Be(command.GroupType);
            gamingGroup.CreatedBy.Should().Be(userId);
            gamingGroup.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
