using System.ComponentModel.DataAnnotations;

namespace Focus.Shared.DTOs.Users
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}