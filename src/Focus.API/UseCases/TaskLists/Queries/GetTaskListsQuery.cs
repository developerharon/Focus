using AutoMapper;
using Focus.API.Data;
using Focus.API.Extensions;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.TaskLists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Focus.API.UseCases.TaskLists.Queries
{
    public class GetTaskListsQuery : IRequest<ResponseDTO<IEnumerable<TaskListDTO>>>
    {
        public class GetTaskListsQueryHandler : IRequestHandler<GetTaskListsQuery, ResponseDTO<IEnumerable<TaskListDTO>>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ApplicationDBContext _dbContext;
            private readonly IMapper _mapper;

            public GetTaskListsQueryHandler(IHttpContextAccessor httpContextAccessor, ApplicationDBContext dbContext, IMapper mapper)
            {
                _httpContextAccessor = httpContextAccessor;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<ResponseDTO<IEnumerable<TaskListDTO>>> Handle(GetTaskListsQuery request, CancellationToken cancellationToken)
            {
                var userID = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserID")?.Value;
                var taskLists = await _dbContext.TaskLists.Where(x => x.UserId == userID).ToListAsync();
                var taskListDtos = taskLists.MapTaskListsToDTOList(_mapper);
                return ResponseDTO<IEnumerable<TaskListDTO>>.Create(Shared.Enums.ResponseType.Success, taskListDtos);
            }
        }
    }
}