using System;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using EGameCafe.Application.Common.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EGameCafe.Application.Common.Exceptions;

namespace EGameCafe.Application.GroupMember.Queries.JoinViaInvitation
{
    public class JoinViaInvitationQuery : IRequest<Result>
    {
        public string token { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<JoinViaInvitationQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdGenerator _idGenerator;
        private readonly IDateTime _dateTime;


        public Handler(IApplicationDbContext context, IIdGenerator idGenerator, IDateTime dateTime)
        {
            _context = context;
            _idGenerator = idGenerator;
            _dateTime = dateTime;
        }

        public async Task<Result> Handle(JoinViaInvitationQuery request, CancellationToken cancellationToken)
        {
            var entry = await _context.GamingGroups.FirstOrDefaultAsync(e => e.SharingLink == request.token);

            if (entry == null)
            {
                throw new NotFoundException("group not found");
            }

            string id = await _idGenerator.BasicIdGenerator(_dateTime);

            var item = new GamingGroupMembers
            {
                GroupMemberId = id,
                GroupId = entry.GamingGroupGroupId,
                UserId = request.UserId,
                Block = false
            };

            _context.GroupMembers.Add(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(item.GroupMemberId, "https://tools.ietf.org/html/rfc7231#section-6.3.1", 200, "Ok");

        }
    }
}
