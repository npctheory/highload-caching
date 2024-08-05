using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Application.DTO;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.SearchUsers;
using Application.Users.Queries.Login;
using MediatR;

namespace Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;

        public UserController(ISender mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement jsonElement)
        {
            string id = jsonElement.GetProperty("id").GetString();
            string password = jsonElement.GetProperty("password").GetString();
            var loginResult = await _mediator.Send(new LoginQuery(id,password)); 
            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpGet("user/get/{id}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] string id)
        {
            var userResult = await _mediator.Send(new GetUserQuery(id));        
            return Ok(userResult);
        }

        [AllowAnonymous]
        [HttpGet("user/search")]
        public async Task<IActionResult> SearchUsersAsync([FromQuery] string first_name, [FromQuery] string second_name)
        {
            var usersResult = await _mediator.Send(new SearchUsersQuery(first_name,second_name));
            return Ok(usersResult);
        }
    }
}