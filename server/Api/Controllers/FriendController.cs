using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Application.DTO;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.SearchUsers;
using Application.Users.Queries.Login;
using MediatR;
using Application.Friends.Queries.ListFriends;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Friends.Queries.DeleteFriend;
using Application.Friends.Queries.SetFriend;

namespace Api.Controllers
{
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ISender _mediator;

        public FriendController(ISender mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("friend/list")]
        public async Task<IActionResult> GetFriends()
        {
            var user_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            try
            {
                var friends = await _mediator.Send(new ListFriendsQuery(user_id));
                return Ok(friends);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPut("friend/delete/{friend_id}")]
        public async Task<IActionResult> DeleteFriend([FromRoute] string friend_id)
        {
            var user_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            try
            {
                await _mediator.Send(new DeleteFriendQuery(user_id, friend_id));
                return Ok(await _mediator.Send(new ListFriendsQuery(user_id)));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPut("friend/set/{friend_id}")]
        public async Task<IActionResult> SetFriend([FromRoute] string friend_id)
        {
            var user_id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            try
            {
                await _mediator.Send(new SetFriendQuery(user_id, friend_id));
                return Ok(await _mediator.Send(new ListFriendsQuery(user_id)));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}