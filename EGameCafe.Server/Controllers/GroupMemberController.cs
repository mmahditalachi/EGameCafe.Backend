using System.Threading.Tasks;
using EGameCafe.Application.GroupMembers.Commands.JoinGroup;
using EGameCafe.Application.GroupMembers.Commands.JoinViaInvitation;
using EGameCafe.Application.GroupMembers.Commands.LeaveGroup;
using EGameCafe.Application.GroupMembers.Queries.SendInvitation;
using EGameCafe.Application.GroupMembers.Queries.GetUserGroups;
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

        [HttpDelete("LeaveGroup")]
        [Authorize]
        public async Task<IActionResult> LeaveGroup(LeaveGroupCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("GetGroupLink/{userId}/{groupId}")]
        [Authorize]
        public async Task<IActionResult> GetGroupLink(string userId, string groupId)
        {
            var command = new SendInvitationQuery { GroupId = groupId, UserId = userId };
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(Url.Action("JoinViaGroupInvitation", "GroupMember", new { token = result, userId = userId }, Request.Scheme)) : BadRequest();
        }

        [HttpPost("JoinViaGroupInvitation/{token}/{userId}")]
        [Authorize]
        public async Task<IActionResult> JoinViaGroupInvitation([FromQuery]string token, string userId)
        {
            var command = new JoinViaInvitationCommand { token = token, UserId = userId };
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

    }
}
