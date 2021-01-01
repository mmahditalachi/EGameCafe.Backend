using EGameCafe.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        //[Authorize]
        public async Task<IActionResult> GetUserFriends(string userId)
        {
            var result = await _userService.GetUserFriends(userId);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("[action]")]
        //[Authorize]
        public async Task<IActionResult> GetUser(string username, string currentUserId)
        {
            var result = await _userService.GetUsers(username, currentUserId);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
