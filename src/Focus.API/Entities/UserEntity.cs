using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Focus.API.Entities
{
    public class UserEntity : IdentityUser
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

        public virtual List<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
        public virtual List<TaskListEntity> TaskLists { get; set; } = new List<TaskListEntity>();
    }
}