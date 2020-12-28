﻿using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Services
{
    public interface IUserService
    {
        Task<Result> MakeFriends(string senderId, string receiverId);
        bool CheckIfFriends(string requestUserId, string targetUserId);
        Task<List<UserFriend>> GetUserFriends(string userId);
        Task<bool> UserExists(string userId);
    }
}
