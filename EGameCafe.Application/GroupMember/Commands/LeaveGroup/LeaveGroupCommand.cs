using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.GroupMember.Commands.LeaveGroup
{
    public class LeaveGroupCommand : IRequest<Result>
    {
        public LeaveGroupCommand(string groupId, string userId)
        {
            GroupId = groupId;
            UserId = userId;
        }

        public string GroupId { get; set; }
        public string UserId { get; set; }

    }

    public class Handler : IRequestHandler<LeaveGroupCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.GroupMembers.FirstOrDefaultAsync(e => e.UserId == request.UserId && e.GroupId == request.GroupId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(GamingGroups), request.GroupId);
            }

            _context.GroupMembers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
