using System.Threading.Tasks;
using EGameCafe.Application.UserSystemInfos.Commands.UpdateUserSystemInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class UserSystemInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserSystemInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("[action]")]
        //[Authorize]
        public async Task<IActionResult> UpdateUserSystemInfo(UpdateUserSystemInfoCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest();
        }
    }
}
