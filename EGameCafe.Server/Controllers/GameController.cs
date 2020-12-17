using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EGameCafe.Application.UsersDetails.Queries.GettUserDetail;
using EGameCafe.Application.Games.Queries.GetAllGames;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllGames(int from, int count, string sortType)
        {
            var query = new GetAllGamesQuery(from, count, sortType);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }
    }
}
