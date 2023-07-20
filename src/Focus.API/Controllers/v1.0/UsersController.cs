﻿using Focus.API.UseCases.Users.Commands;
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

        [AllowAnonymous, HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserDTO dto)
        {
            var result = await _mediator.Send(new CreateUserCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous, HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [AllowAnonymous, HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO dto)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _mediator.Send(new LogoutCommand(HttpContext));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}