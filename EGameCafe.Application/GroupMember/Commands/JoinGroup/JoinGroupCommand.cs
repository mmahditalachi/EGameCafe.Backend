using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.GroupMember.Commands.JoinGroup
{
    public class JoinGroupCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }

    public class Handler : IRequestHandler<JoinGroupCommand, Result>
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

        public async Task<Result> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entry = new GamingGroupMembers()
                {
                    GroupId = request.GroupId,
                    UserId = request.UserId
                };

                entry.GroupMemberId = await _idGenerator.BasicIdGenerator(_dateTime, nameof(JoinGroupCommand));

                _context.GroupMembers.Add(entry);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(entry.GroupMemberId, "https://tools.ietf.org/html/rfc7231#section-6.3.1", 201, "Created");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
