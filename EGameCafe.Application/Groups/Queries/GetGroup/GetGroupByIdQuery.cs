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

namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdQuery : IRequest<GetGroupByIdDto>
    {
        public string GroupId { get; set; }

        public GetGroupByIdQuery(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class Handler : IRequestHandler<GetGroupByIdQuery, GetGroupByIdDto>
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

        public async Task<GetGroupByIdDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = new GetGroupByIdDto();

            string cacheKey = request.GroupId + "GetGroupByIdQuery";

            if (_cache.TryGetValue(cacheKey, out entity))
            {
                return entity;
            }

            entity = await _context.Group
                   .Where(e => e.GroupId == request.GroupId)
                   .ProjectTo<GetGroupByIdDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

            if (entity != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, entity, cacheEntryOptions);

                return entity;
            }

            throw new NotFoundException(nameof(GetGroupByIdQuery), request.GroupId);
        }
    }
}
