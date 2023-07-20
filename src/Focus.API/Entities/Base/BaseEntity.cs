using System.ComponentModel.DataAnnotations;

namespace Focus.API.Entities.Base
{
    public class BaseEntity
    {
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set;}
    }
}