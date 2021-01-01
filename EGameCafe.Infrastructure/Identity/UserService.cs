using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models.FriendRequest;
using EGameCafe.Domain.Entities;
using EGameCafe.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _cache;


        public UserService(UserManager<ApplicationUser> userManager, IApplicationDbContext context, IMemoryCache cache)
        {
            _userManager = userManager;
            _context = context;
            _cache = cache;
        }

        public async Task<Result> MakeFriends(string senderId, string receiverId)
        {
            if (await UserExists(senderId) && await UserExists(receiverId) && !CheckIfFriends(senderId, receiverId))
            {
                var userFriend = new UserFriend
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = senderId,
                    FriendId = receiverId
                };

                _context.UserFriends.Add(userFriend);

                await _context.SaveChangesAsync();

                return Result.Success();
            }

            return Result.Failure("user not found", "user not found exception");
        }

        public async Task<List<UserFriend>> GetUserFriends(string userId)
        {
            string cacheKey = nameof(GetUserFriends) + userId;

            if (_cache.TryGetValue(cacheKey, out List<UserFriend> cacheData))
            {
                return cacheData;
            }

            var friends = await _context.UserFriends.Where(e => e.FriendId == userId || e.UserId == userId).ToListAsync();

            if (friends.Any())
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(cacheKey, friends, cacheEntryOptions);

                return friends;
            }

            throw new NotFoundException(nameof(GetUserFriends), userId);

        }

        public bool CheckIfFriends(string requestUserId, string targetUserId)
        {
            return _context.UserFriends.Any(uf => (uf.UserId == requestUserId && uf.FriendId == targetUserId) || (uf.UserId == targetUserId && uf.FriendId == requestUserId));
        }

        public async Task<List<UserSearchModel>> GetUsers(string username, string currentUserId)
        {
            var users = await _context.UserDetails
                .Where(e=>e.Username.ToLower().Contains(username.ToLower()))
                .Take(5)
                .Select(e=>new UserSearchModel { UserId = e.UserId, Username= e.Username })
                .ToListAsync();

            var currentuser = users.FirstOrDefault(e => e.UserId == currentUserId);

            if(currentuser != null)
            {
                users.Remove(currentuser);
            }

            if(users != null)
            {
                return users;
            }

            return new List<UserSearchModel>();
        }
        
        public async Task<bool> UserExists(string userId) => await _userManager.FindByIdAsync(userId) != null ? true : false;
    }
}
