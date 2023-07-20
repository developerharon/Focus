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
    }
}