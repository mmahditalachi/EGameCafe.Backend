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

namespace EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo
{
    public class GetUserDashboardInfoQuery : IRequest<GetUserDashboardInfoVm>
    {
        public GetUserDashboardInfoQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<GetUserDashboardInfoQuery, GetUserDashboardInfoVm>
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
        public async Task<GetUserDashboardInfoVm> Handle(GetUserDashboardInfoQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.UserId + nameof(GetUserDashboardInfoQuery);

            if (_cache.TryGetValue(cacheKey, out GetUserDashboardInfoVm cacheData))
            {
                return cacheData;
            }

            var vm = new GetUserDashboardInfoVm();

            vm.GameList = await _context.UserGames.Include(e => e.Game)
                .Where(e => e.UserId == request.UserId).ProjectTo<GetUserDashboardGameDto>(_mapper.ConfigurationProvider).ToListAsync();

            vm.SystemInfo = await _context.UserSystemInfo
                .Where(e => e.UserId == request.UserId).ProjectTo<GetUserDashboardSystemDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            vm.ActivityList = await _context.Activity
                .Where(e => e.UserId == request.UserId).ProjectTo<GetUserDashboardActivityDto>(_mapper.ConfigurationProvider).ToListAsync();

            vm.PersonalInfo = await _context.UserDetails
                .Where(e => e.UserId == request.UserId).ProjectTo<GetUserDashboardPersonalDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (vm.GameList.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, vm, cacheEntryOptions);

                return vm;
            }

            throw new NotFoundException(nameof(GetUserDashboardInfoQuery), request);
        }
    }
}
