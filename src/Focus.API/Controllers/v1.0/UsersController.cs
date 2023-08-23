using Focus.API.UseCases.Users.Commands;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Response DTO with the created User ID</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> CreateUser(CreateUserDTO dto)
        {
            var result = await _mediator.Send(new CreateUserCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<LoginResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type= typeof(ResponseDTO<LoginResponseDTO>))]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<LoginResponseDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO<LoginResponseDTO>))]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO dto)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO<string>))]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutCommand(HttpContext));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}