using System.ComponentModel.DataAnnotations;

namespace Focus.Shared.DTOs.TaskLists
{
    public class CreateOrUpdateTaskListDTO
    {
        public Guid Id { get; set; }
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

    }
}