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

namespace EGameCafe.Application.GroupMembers.Commands.JoinViaInvitation
{
    public class JoinViaInvitationCommand : IRequest<Result>
    {
        public string token { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<JoinViaInvitationCommand, Result>
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

        public async Task<Result> Handle(JoinViaInvitationCommand request, CancellationToken cancellationToken)
        {

            var entry = await _context.Group.FirstOrDefaultAsync(e => e.SharingLink == request.token);

            if (entry == null)
            {
                throw new NotFoundException("group not found");
            }

            if (_context.GroupMember.Any(e=>e.UserId == request.UserId && e.GroupId == entry.GroupId))
            {
                throw new DuplicateUserException($"DuplicateUser : {request.UserId}");
            }

            string id = Guid.NewGuid().ToString();

            var item = new GroupMember
            {
                GroupMemberId = id,
                GroupId = entry.GroupId,
                UserId = request.UserId,
                IsBlock = false
            };

            _context.GroupMember.Add(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(item.GroupMemberId, "https://tools.ietf.org/html/rfc7231#section-6.3.1", 200, "Ok");

        }
    }
}
