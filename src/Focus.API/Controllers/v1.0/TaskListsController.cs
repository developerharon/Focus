using Focus.API.UseCases.TaskLists.Commands;
using Focus.API.UseCases.TaskLists.Queries;
using Focus.Shared.DTOs.TaskLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskListsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TaskListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var result = await _mediator.Send(new GetTaskListsQuery());

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateList(CreateOrUpdateTaskListDTO dto)
        {
            var result = await _mediator.Send(new CreateOrUpdateTaskListCommand(dto));

            if (result.ResponseType == Shared.Enums.ResponseType.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}