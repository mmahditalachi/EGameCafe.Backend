using EGameCafe.Application.GamingGroup.Queries.GetAllGroups;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.GamingGroup.Queries
{
    using static Testing;

    public class GetAllGroupsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllGroups()
        {
            await AddAsync(new GamingGroups { 
                GamingGroupGroupId = await GenerateRandomId(),
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup
            });

            var query = new GetAllGroupsQuery(0, 10, "");

            var result = await SendAsync(query);

            result.List.Should().HaveCount(1);
            result.TotalGroups.Should().Equals(1);
        }
    }
}
