using AutoMapper;
using AutoMapper.QueryableExtensions;
using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.GroupMember.Queries.GetUserGroups
{
    public class GetUserGroupsQuery : IRequest<GetUserGroupsVm>
    {
        public GetUserGroupsQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<GetUserGroupsQuery, GetUserGroupsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMemoryCache cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<GetUserGroupsVm> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.UserId + "GetUserGroupsQuery";

            if (_cache.TryGetValue(cacheKey, out GetUserGroupsVm entity))
            {
                return entity;
            }

            entity = await _context.GroupMembers
                   .Where(e => e.UserId == request.UserId)
                   .ProjectTo<GetUserGroupsVm>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

            if (entity != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, entity, cacheEntryOptions);

                return entity;
            }

            throw new NotFoundException(nameof(GetUserGroupsQuery), request.UserId);

        }
    }
}
