﻿using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GamingGroup.Commands.CreateGroup;
using EGameCafe.Application.GroupMember.Commands.JoinGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace Application.IntegrationTests.GroupMember.Commands
{
    using static Testing;

    public class JoinGroupTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new JoinGroupCommand("","");

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldJoinToGroupAndReturnSucceeded()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = await GenerateRandomId();

            var item = new GamingGroups
            {
                GamingGroupGroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup
            };

            await AddAsync(item);

            var command = new JoinGroupCommand
            (
                userId,
                groupId
            );

            var result = await SendAsync(command);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Status.Should().Equals(201);
        }

        [Test]
        public async Task ShouldJoinMemberToGroup()
        {
            var userId = await RunAsDefaultUserAsync();

            var groupId = await GenerateRandomId();

            var item = new GamingGroups
            {
                GamingGroupGroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup
            };

            await AddAsync(item);

            var command = new JoinGroupCommand
            (
                userId,
                groupId
            );

            var result = await SendAsync(command);

            var gamingGroup = await FindAsync<GamingGroupMembers>(result.Id);

            gamingGroup.Should().NotBeNull();
            gamingGroup.GroupId.Should().Be(groupId);
            gamingGroup.UserId.Should().Be(userId);
            //gamingGroup..Should().Be(userId);
            //gamingGroup.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }

    }
}
