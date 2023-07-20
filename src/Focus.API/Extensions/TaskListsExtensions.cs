using AutoMapper;
using Focus.API.Entities;
using Focus.Shared.DTOs.TaskLists;

namespace Focus.API.Extensions
{
    public static class TaskListsExtensions
    {
        public static IEnumerable<TaskListDTO> MapTaskListsToDTOList(this IEnumerable<TaskListEntity> entities, IMapper mapper)
        {
            var dtoList = new List<TaskListDTO>(entities.Count());
            foreach (var entity in entities)
            {
                dtoList.Add(entity.MapTaskListEntityToDTO(mapper));
            }
            return dtoList;
        }

        public static TaskListDTO MapTaskListEntityToDTO(this TaskListEntity taskList, IMapper mapper)
        {
            return mapper.Map<TaskListDTO>(taskList);
        }
    }
}