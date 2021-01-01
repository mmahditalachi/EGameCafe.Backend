using AutoMapper;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Application.Groups.Queries.GetGroup;
using EGameCafe.Application.GroupMembers.Queries.GetUserGroups;
using EGameCafe.Domain.Entities;
using NUnit.Framework;
using System;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        //[Test]
        //public void ShouldHaveValidConfiguration()
        //{
        //    _configuration.AssertConfigurationIsValid();
        //}

        [Test]
        [TestCase(typeof(Group), typeof(GetUserGroupsDto))]
        [TestCase(typeof(Group), typeof(GetAllGroupsDto))]
        [TestCase(typeof(Group), typeof(GetGroupByIdInfoDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
