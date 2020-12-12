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

namespace EGameCafe.Application.Games.Queries.GetAllGames
{
    public class GetAllGamesQuery : IRequest<GetAllGamesVm>
    {
        public GetAllGamesQuery(int from, int count, string sortType)
        {
            From = from;
            Count = count;
            this.sortType = sortType;
        }

        public int From { get; set; }
        public int Count { get; set; }
        public string sortType { get; set; }
    }

    public class Handler : IRequestHandler<GetAllGamesQuery, GetAllGamesVm>
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

        public async Task<GetAllGamesVm> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"{request.From}{request.Count}{request.sortType}{nameof(GetAllGamesQuery)}";

            if (_cache.TryGetValue(cacheKey, out GetAllGamesVm cacheData))
            {
                return cacheData;
            }

            var vm = new GetAllGamesVm();

            vm.TotalGames = _context.Group.Count();

            switch (request.sortType)
            {
                case "gamename":
                    vm.List = await _context.Game.OrderBy(e => e.GameName).Skip(request.From).Take(request.Count)
                                        .ProjectTo<GetAllGamesDto>(_mapper.ConfigurationProvider).ToListAsync();
                    break;

                default:
                    vm.List = await _context.Game.Skip(request.From).Take(request.Count)
                                        .ProjectTo<GetAllGamesDto>(_mapper.ConfigurationProvider).ToListAsync();
                break;
            }

            if (vm.List.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, vm, cacheEntryOptions);

                return vm;
            }

            throw new NotFoundException(nameof(GetAllGamesQuery), request);

        }
    }
}
