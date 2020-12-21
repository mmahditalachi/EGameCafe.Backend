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

namespace EGameCafe.Application.Games.Queries.GetUserGamesById
{
    public class GetUserGamesByIdQuery : IRequest<GetUserGamesByIdVm>
    {
        public GetUserGamesByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<GetUserGamesByIdQuery, GetUserGamesByIdVm>
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
        public async Task<GetUserGamesByIdVm> Handle(GetUserGamesByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.UserId + nameof(GetUserGamesByIdQuery);

            if (_cache.TryGetValue(cacheKey, out GetUserGamesByIdVm cacheData))
            {
                return cacheData;
            }

            var vm = new GetUserGamesByIdVm();

            vm.List = await _context.UserGames.Include(e=>e.Game)
                .Where(e=>e.UserId == request.UserId).ProjectTo<GetUserGamesByIdDto>(_mapper.ConfigurationProvider).ToListAsync();

            vm.UserId = request.UserId;

            if (vm.List.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, vm, cacheEntryOptions);

                return vm;
            }

            throw new NotFoundException(nameof(GetUserGamesByIdQuery), request);
        }
    }
}
