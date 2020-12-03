using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Groups.Commands.CreateGroup;
using EGameCafe.Application.Groups.Commands.Removegroup;
using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Application.Groups.Queries.GetGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
namespace Application.IntegrationTests.Groups.Queries
{
    using static Testing;

    public class GetGroupByIdTest : TestBase
    {
        [Test]
        public async Task ShouldReturnGroupById()
        {
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

            var query = new GetGroupByIdQuery(groupId);

            var result = await SendAsync(query);

            result.GroupName.Should().Equals(item.GroupName);
            result.GroupType.Should().Equals(item.GroupType);
            
        }
    }
}
