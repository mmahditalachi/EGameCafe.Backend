using System.Threading.Tasks;
using EGameCafe.Application.GroupMember.Commands.JoinGroup;
using EGameCafe.Application.GroupMember.Commands.LeaveGroup;
using EGameCafe.Application.GroupMember.Queries.GetUserGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class GroupMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("JoinGroup")]
        [Authorize]
        public async Task<IActionResult> JoinGroup(JoinGroupCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest();
        }


        [HttpGet("GetUserGroups/{userId}", Name = nameof(GetUserGroups))]
        [Authorize]
        public async Task<IActionResult> GetUserGroups(string userId)
        {
            var query = new GetUserGroupsQuery(userId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpDelete("LeaveGroup/{groupId}/{userId}")]
        [Authorize]
        public async Task<IActionResult> LeaveGroup(string groupId, string userId)
        {
            var command = new LeaveGroupCommand(groupId, userId);
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }
    }
}
