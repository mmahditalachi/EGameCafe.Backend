using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EGameCafe.Application.Games.Queries.GetAllGames;
using EGameCafe.Application.Games.Queries.GetUserGamesById;
using EGameCafe.Application.Dashboard.Queries.GetUserDashboardInfo;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        //[Authorize]
        public async Task<IActionResult> GetUserDashboard(string userId)
        {
            var query = new GetUserDashboardInfoQuery(userId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
