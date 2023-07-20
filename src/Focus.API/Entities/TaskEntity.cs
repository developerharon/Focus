using Focus.API.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Focus.API.Entities
{
    public class TaskEntity : BaseEntity
    {
        [Required, StringLength(300, MinimumLength = 10)]
        public string Name { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public bool IsCompleted { get; set; }

        // Foreign keys
        public Guid? ListId { get; set; }
        public string? UserId { get; set; }

        // Navigation properties 
        public TaskListEntity List { get; set; }
        public UserEntity User { get; set; }
    }
}