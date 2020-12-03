using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Groups.Commands.Removegroup
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
            var entity = await _context.Group.FirstOrDefaultAsync(e => e.GroupId == request.GroupId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Group), request.GroupId);
            }

            _context.Group.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
