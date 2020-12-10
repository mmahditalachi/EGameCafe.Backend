using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EGameCafe.Application.UsersDetails.Queries.GettUserDetail;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class UserDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var query = new GetUserDetailQuery(userId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
