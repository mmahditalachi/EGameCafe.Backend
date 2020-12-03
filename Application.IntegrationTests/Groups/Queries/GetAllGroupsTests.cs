using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Groups.Queries
{
    using static Testing;

    public class GetAllGroupsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllGroups()
        {
            await AddAsync(new Group { 
                GroupId = Guid.NewGuid().ToString(),
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = await GenerateSHA1Hash()
            });

            var query = new GetAllGroupsQuery(0, 10, "");

            var result = await SendAsync(query);

            result.List.Should().HaveCount(1);
            result.TotalGroups.Should().Equals(1);
        }

        [Test]
        public async Task ShouldReturnOnlyPublicGroup()
        {
            await AddAsync(new Group
            {
                GroupId = Guid.NewGuid().ToString(),
                GroupName = "gpTest",
                GroupType = GroupType.privateGroup,
                SharingLink = await GenerateSHA1Hash()
            });

            await AddAsync(new Group
            {
                GroupId = Guid.NewGuid().ToString(),
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup,
                SharingLink = await GenerateSHA1Hash()
            });

            var query = new GetAllGroupsQuery(0, 10, "");

            var result = await SendAsync(query);

            result.List.Should().HaveCount(1);

            result.List.Should().OnlyContain(e => e.GroupType == GroupType.publicGroup);

            result.TotalGroups.Should().Equals(1);
        }
    }
}
