using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models.FriendRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IFriendRequestService
    {
        Task<List<GetAllFriendRequestDto>> GetAllFriendRequest(string userId);
        Task<Result> AcceptAsync(FriendRequestModel model);
        Task<Result> CreateAsync(FriendRequestModel model);
        Task<Result> DeclineAsync(FriendRequestModel model);
        Task<Result> Remove(RemoveFriendRequestModel model);
    }
}
