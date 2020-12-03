using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.GroupMembers.Queries.SendInvitation
{
    public class SendInvitationQuery : IRequest<string>
    {
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }

    public class Handler : IRequestHandler<SendInvitationQuery, string>
    {

        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(SendInvitationQuery request, CancellationToken cancellationToken)
        {
            var entry = await _context.Group.FirstOrDefaultAsync(e=>e.GroupId == request.GroupId);

            if(entry == null)
            {
                throw new NotFoundException();
            }

            return entry.SharingLink;
        }
    }
}
