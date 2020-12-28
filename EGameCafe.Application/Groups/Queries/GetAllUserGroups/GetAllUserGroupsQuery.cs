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


namespace EGameCafe.Application.Groups.Queries.GetAllUserGroups
{
    public class GetAllUserGroupsQuery : IRequest<GetAllUserGroupsVm>
    {
        public GetAllUserGroupsQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<GetAllUserGroupsQuery, GetAllUserGroupsVm>
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

        public async Task<GetAllUserGroupsVm> Handle(GetAllUserGroupsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.ToString() + nameof(GetAllUserGroupsQuery);

            if (_cache.TryGetValue(cacheKey, out GetAllUserGroupsVm cacheData))
            {
                return cacheData;
            }

            var vm = new GetAllUserGroupsVm();

            vm.List = await _context.Group.Include(e => e.Game).Include(e => e.GroupMembers).Where(e => e.GroupMembers.Any(p => p.UserId == request.UserId && p.GroupId == e.GroupId))
                .OrderBy(e => e.GroupName).ProjectTo<GetAllUserGroupsDto>(_mapper.ConfigurationProvider).ToListAsync();

            if (vm.List.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, vm, cacheEntryOptions);

                return vm;
            }

            throw new NotFoundException(nameof(GetAllUserGroupsQuery), request);

        }
    }
}
