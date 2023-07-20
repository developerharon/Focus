using Focus.API.UseCases.Users.Commands;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserDTO dto)
        {
            var command = new CreateUserCommand(dto);
            var result = await _mediator.Send(command);

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}