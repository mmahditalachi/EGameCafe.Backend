using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.Activities.Commands.CreateActivity
{
    public class CreateActivityCommand : IRequest<Result>
    {
        public string ActivityTitle { get; set; }
        public string ActivityText { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<CreateActivityCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = new Activity()
                {
                    ActivityId = Guid.NewGuid().ToString(),
                    ActivityTitle = request.ActivityTitle,
                    ActivityText = request.ActivityText,
                    UserId = request.UserId
                };

                _context.Activity.Add(item);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(item.ActivityId, "https://tools.ietf.org/html/rfc7231#section-6.3.1", 201, "Created");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
