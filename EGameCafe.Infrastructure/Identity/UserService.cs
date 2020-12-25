using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using EGameCafe.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result> MakeFriends(string senderId, string receiverId)
        {
            if (await UserExists(senderId) && await UserExists(receiverId) && !CheckIfFriends(senderId, receiverId))
            {
                var userFriend = new UserFriend
                {
                    UserId = senderId,
                    FriendId = receiverId
                };

                _context.UserFriends.Add(userFriend);

                await _context.SaveChangesAsync();

                return Result.Success();
            }

            return Result.Failure("user not found", "user not found exception");
        }

        public bool CheckIfFriends(string requestUserId, string targetUserId)
        {
            return _context.UserFriends.Any(uf => (uf.UserId == requestUserId && uf.FriendId == targetUserId) || (uf.UserId == targetUserId && uf.FriendId == requestUserId));
        }
        
        public async Task<bool> UserExists(string userId) => await _userManager.FindByIdAsync(userId) != null ? true : false;
    }
}
