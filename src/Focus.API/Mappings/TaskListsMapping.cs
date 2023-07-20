using AutoMapper;
using Focus.API.Entities;
using Focus.Shared.DTOs.TaskLists;

namespace Focus.API.Mappings
{
    public class TaskListsMapping : Profile
    {
        public TaskListsMapping()
        {
            CreateMap<CreateOrUpdateTaskListDTO, TaskListEntity>();
            CreateMap<TaskListEntity, TaskListDTO>();
        }
    }
}