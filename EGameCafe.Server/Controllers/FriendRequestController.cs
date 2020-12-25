using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Models.FriendRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<IActionResult> CreateFriendRequest(FriendRequestModel model)
        {
            var result = await _friendRequestService.CreateAsync(model);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestModel model)
        {
            var result = await _friendRequestService.AcceptAsync(model);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<IActionResult> DeclineFriendRequest(FriendRequestModel model)
        {
            var result = await _friendRequestService.DeclineAsync(model);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost("[action]")]
        //[Authorize]
        public async Task<IActionResult> RemoveFriendRequest(RemoveFriendRequestModel model)
        {
            var result = await _friendRequestService.Remove(model);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
