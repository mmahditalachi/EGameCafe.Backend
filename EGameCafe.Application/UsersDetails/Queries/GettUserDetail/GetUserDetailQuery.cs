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

namespace EGameCafe.Application.UsersDetails.Queries.GettUserDetail
{
    public class GetUserDetailQuery : IRequest<GetUserDetailDto>
    {
        public GetUserDetailQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<GetUserDetailQuery, GetUserDetailDto>
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

        public async Task<GetUserDetailDto> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = new GetUserDetailDto();

            string cacheKey = request.UserId + nameof(GetUserDetailQuery);

            if (_cache.TryGetValue(cacheKey, out entity))
            {
                return entity;
            }

            entity = await _context.UserDetails
                   .Where(e => e.UserId == request.UserId)
                   .ProjectTo<GetUserDetailDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

            if (entity != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, entity, cacheEntryOptions);

                return entity;
            }

            throw new NotFoundException(nameof(GetUserDetailQuery), request.UserId);
        }
    }
}
