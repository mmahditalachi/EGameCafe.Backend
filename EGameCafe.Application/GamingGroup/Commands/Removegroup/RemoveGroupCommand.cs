using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.GamingGroup.Commands.Removegroup
{
    public class RemoveGroupCommand : IRequest<Result>
    {
        public string GroupId { get; set; }
    }

    public class Handler : IRequestHandler<RemoveGroupCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.GamingGroups.FirstOrDefaultAsync(e => e.GamingGroupGroupId == request.GroupId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(GamingGroups), request.GroupId);
            }

            _context.GamingGroups.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
