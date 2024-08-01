using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Services;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }


        [HttpGet("friend/list")]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                var friends = await _friendsService.GetFriendsForCurrentUserAsync(HttpContext);
                return Ok(friends);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
