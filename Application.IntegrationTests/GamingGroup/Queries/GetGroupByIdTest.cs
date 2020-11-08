using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.GamingGroup.Commands.CreateGroup;
using EGameCafe.Application.GamingGroup.Commands.Removegroup;
using EGameCafe.Application.GamingGroup.Queries.GetAllGroups;
using EGameCafe.Application.GamingGroup.Queries.GetGroup;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
namespace Application.IntegrationTests.GamingGroup.Queries
{
    using static Testing;

    public class GetGroupByIdTest : TestBase
    {
        [Test]
        public async Task ShouldReturnGroupById()
        {
            var groupId = await GenerateRandomId();

            var item = new GamingGroups
            {
                GamingGroupGroupId = groupId,
                GroupName = "gpTest",
                GroupType = GroupType.publicGroup
            };

            await AddAsync(item);

            var query = new GetGroupByIdQuery(groupId);

            var result = await SendAsync(query);

            result.GroupName.Should().Equals(item.GroupName);
            result.GroupType.Should().Equals(item.GroupType);
            
        }
    }
}
