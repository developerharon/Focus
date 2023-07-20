using AutoMapper;
using Focus.API.Data;
using Focus.API.Entities;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.TaskLists;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Focus.API.UseCases.TaskLists.Commands
{
    public class CreateOrUpdateTaskListCommand : IRequest<ResponseDTO<Guid>>
    {
        private readonly CreateOrUpdateTaskListDTO DTO;

        public CreateOrUpdateTaskListCommand(CreateOrUpdateTaskListDTO dto)
        {
            DTO = dto;
        }

        public class CreateOrUpdateTaskListCommandHandler : IRequestHandler<CreateOrUpdateTaskListCommand, ResponseDTO<Guid>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ApplicationDBContext _dbContext;
            private readonly IMapper _mapper;

            public CreateOrUpdateTaskListCommandHandler(IHttpContextAccessor contextAccessory, ApplicationDBContext dbContext, IMapper mapper)
            {
                _httpContextAccessor = contextAccessory;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<ResponseDTO<Guid>> Handle(CreateOrUpdateTaskListCommand request, CancellationToken cancellationToken)
            {

                var userID = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserID")?.Value;

                if (request.DTO.Id == Guid.Empty)
                    return await CreateNewTaskList(request, userID);

                var taskList = await _dbContext.TaskLists.Where(x => x.Id == request.DTO.Id).FirstOrDefaultAsync();

                if (taskList == null)
                    return await CreateNewTaskList(request, userID);

                if (taskList.UserId != userID)
                    return ResponseDTO<Guid>.Create(Shared.Enums.ResponseType.Error, Guid.Empty, "Error when updating the task.");

                taskList = _mapper.Map(request.DTO, taskList);
                _dbContext.TaskLists.Update(taskList);
                await _dbContext.SaveChangesAsync();

                return ResponseDTO<Guid>.Create(Shared.Enums.ResponseType.Success, taskList.Id);

            }

            private async Task<ResponseDTO<Guid>> CreateNewTaskList(CreateOrUpdateTaskListCommand request, string? userID)
            {
                var taskList = _mapper.Map<TaskListEntity>(request.DTO);
                taskList.UserId = userID;
                await _dbContext.TaskLists.AddAsync(taskList);
                await _dbContext.SaveChangesAsync();
                return ResponseDTO<Guid>.Create(Shared.Enums.ResponseType.Success, taskList.Id);
            }
        }
    }
}