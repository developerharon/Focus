using Focus.API.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Focus.API.Entities
{
    public class TaskListEntity : BaseEntity
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        // Foreign keys
        public string UserId { get; set; }

        // Navigation properties
        public virtual List<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
        public virtual UserEntity User { get; set; }
    }
}