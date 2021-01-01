using System.Threading.Tasks;
using EGameCafe.Application.Groups.Commands.CreateGroup;
using EGameCafe.Application.Groups.Commands.Removegroup;
using EGameCafe.Application.Groups.Queries.GetAllGroups;
using EGameCafe.Application.Groups.Queries.GetAllUserGroups;
using EGameCafe.Application.Groups.Queries.GetGroup;
using EGameCafe.Application.Groups.Queries.SendInvitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGameCafe.Server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateGroup")]
        [Authorize]
        public async Task<IActionResult> CreateGroup(CreateGroupCommand command)
        {
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest();
        }

        [HttpGet("GetGroup/{groupId}", Name = nameof(GetGroup))]
        [Authorize]
        public async Task<IActionResult> GetGroup(string groupId)
        {
            var query = new GetGroupByIdQuery(groupId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("GetAllGroups/{from}/{count}/{sortType}")]
        [Authorize]
        public async Task<IActionResult> GetAllGroups(int from, int count, string sortType)
        {
            var query = new GetAllGroupsQuery(from,count,sortType);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("GetAllUserGroups/{userId}")]
        //[Authorize]
        public async Task<IActionResult> GetAllUserGroups(string userId)
        {
            var query = new GetAllUserGroupsQuery(userId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpDelete("RemoveGroup/{groupId}")]
        [Authorize]
        public async Task<IActionResult> RemoveGroup(string groupId)
        {
            var command = new RemoveGroupCommand { GroupId = groupId };
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpGet("GetGroupLink/{userId}/{groupId}")]
        [Authorize]
        public async Task<IActionResult> GetGroupLink(string userId, string groupId)
        {
            var command = new SendInvitationQuery { GroupId = groupId };
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult)Ok(Url.Action("JoinViaGroupInvitation", "GroupMember", new { userId = userId , token = result,}, Request.Scheme)) : BadRequest();
        }
    }
}
