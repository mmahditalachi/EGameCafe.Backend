using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<Result>
    {
        public string GroupName { get; set; }
        public GroupType GroupType { get; set; }
        public string GameId { get; set; }
    }

    public class Handler : IRequestHandler<CreateGroupCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdGenerator _idGenerator;

        public Handler(IApplicationDbContext context, IIdGenerator idGenerator)
        {
            _context = context;
            _idGenerator = idGenerator;
        }

        public async Task<Result> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sharingLink = await _idGenerator.SHA1hashGenerator(request.GroupName);

                var game = await _context.Game.FirstOrDefaultAsync(e => e.GameId == request.GameId);

                if(game == null)
                {
                    throw new NotFoundException();
                }

                var entry = new Group() 
                { 
                    GroupName = request.GroupName,
                    GroupType = request.GroupType,
                    SharingLink = sharingLink,
                    Game = game
                };

                entry.GroupId = Guid.NewGuid().ToString();

                _context.Group.Add(entry);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(entry.GroupId,"https://tools.ietf.org/html/rfc7231#section-6.3.1", 201, "Created");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
