using AutoMapper;
using AutoMapper.QueryableExtensions;
using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Groups.Queries.GetGroup
{
    public class GetGroupByIdQuery : IRequest<GetGroupByIdVm>
    {
        public string GroupId { get; set; }

        public GetGroupByIdQuery(string groupId)
        {
            GroupId = groupId;
        }
    }

    public class Handler : IRequestHandler<GetGroupByIdQuery, GetGroupByIdVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;


        public Handler(IApplicationDbContext context, IMemoryCache cache, IMapper mapper, LinkGenerator linkGenerator)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        public async Task<GetGroupByIdVm> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.GroupId + "GetGroupByIdQuery";

            if (_cache.TryGetValue(cacheKey, out GetGroupByIdVm cacheData))
            {
                return cacheData;
            }

            var vm = new GetGroupByIdVm();

            vm.GroupInfo = await _context.Group
                   .Include(e => e.Game)
                   .Where(e => e.GroupId == request.GroupId)
                   .ProjectTo<GetGroupByIdInfoDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync();

            vm.GroupMembers = await _context.GroupMember
                    .Where(e => e.GroupId == request.GroupId)
                    .ProjectTo<GetGroupByIdGroupMemberDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                    

            //entity.SharingLink = _linkGenerator
            //       .GetUriByAction(_currentUser.HttpContext, "ResetPassword", "Auth", new { userId = user.Id, token = token });

            if (vm != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, vm, cacheEntryOptions);

                return vm;
            }

            throw new NotFoundException(nameof(GetGroupByIdQuery), request.GroupId);
        }
    }
}
