using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models.FriendRequest;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using EGameCafe.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Identity
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _userService;

        public FriendRequestService(IApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result> AcceptAsync(FriendRequestModel model)
        {
            if (RequestExists(model.SenderId, model.ReceiverId))
            {
                if (await _userService.UserExists(model.SenderId) && await _userService.UserExists(model.ReceiverId))
                {
                    var friendRequest = await _context.FriendRequest.FirstOrDefaultAsync(fr => fr.ReceiverId == model.ReceiverId && fr.SenderId == model.SenderId);

                    friendRequest.FriendRequestStatus = FriendRequestStatus.Accepted;

                    var result = await _userService.MakeFriends(model.SenderId, model.ReceiverId);

                    await _context.SaveChangesAsync();

                    return Result.Success(friendRequest.Id);
                }

                return Result.Failure("Request has been sent", "Request has been sent");
            }

            return Result.Failure("user not found", "user not found");
        }

        public async Task<Result> CreateAsync(FriendRequestModel model)
        {
            if (!RequestExists(model.SenderId, model.ReceiverId))
            {
                if (await _userService.UserExists(model.SenderId) && await _userService.UserExists(model.ReceiverId))
                {
                    var friendRequest = new FriendRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        SenderId = model.SenderId,
                        ReceiverId = model.ReceiverId,
                        FriendRequestStatus = FriendRequestStatus.Pending
                    };

                    _context.FriendRequest.Add(friendRequest);

                    await _context.SaveChangesAsync();

                    return Result.Success(friendRequest.Id);
                }

                return Result.Failure("user not found", "user not found");
            }

            return Result.Failure("Request has been sended", "Request has been sended");
        }

        public async Task<Result> DeclineAsync(FriendRequestModel model)
        {
            if (await _userService.UserExists(model.SenderId) && await _userService.UserExists(model.ReceiverId))
            {
                if (RequestExists(model.SenderId, model.ReceiverId))
                {
                    var friendRequest = _context.FriendRequest.FirstOrDefault(fr => fr.ReceiverId == model.ReceiverId && fr.SenderId == model.SenderId);

                    _context.FriendRequest.Remove(friendRequest);

                    await _context.SaveChangesAsync();

                    return Result.Success(friendRequest.Id);
                }

                return Result.Failure("Request Not Found", "Request Not Found");
            }

            return Result.Failure("User Not Found", "User Not Found");
        }

        public async Task<List<GetAllFriendRequestDto>> GetAllFriendRequest(string userId)
        {
            if(await _userService.UserExists(userId))
            {
                 return await _context.FriendRequest
                    .Where(e => e.ReceiverId == userId && e.FriendRequestStatus == FriendRequestStatus.Pending)
                    .Select(e=> new GetAllFriendRequestDto { Id = e.Id,  Fullname = e.Sender.Fullname, Username = e.Sender.Username, UserId = e.Sender.UserId, ProfileImage = e.Sender.ProfileImage })
                    .ToListAsync();
            }

            throw new NotFoundException(nameof(GetAllFriendRequest));
        }

        public async Task<Result> Remove(RemoveFriendRequestModel model)
        {
            if (await _userService.UserExists(model.SenderId))
            {
                var friendRequest = await _context.FriendRequest.FirstOrDefaultAsync(e => e.SenderId == model.SenderId && e.Id == model.RequestId);

                if (friendRequest != null)
                {
                    _context.FriendRequest.Remove(friendRequest);

                    await _context.SaveChangesAsync();

                    return Result.Success(friendRequest.Id);
                }

                return Result.Failure("Request Not Found", "Request Not Found");
            }

            return Result.Failure("User Not Found", "User Not Found");
        }

        public bool RequestExists(string senderId, string receiverId) => _context.FriendRequest.Any(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);

    }
}
