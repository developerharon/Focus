using System.ComponentModel.DataAnnotations;

namespace Focus.Shared.DTOs.Users
{
    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "AccessToken is required")]
        public string AccessToken { get; set; }
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}