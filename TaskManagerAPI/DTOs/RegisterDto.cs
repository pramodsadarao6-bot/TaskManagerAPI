using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";   // नवीन — डिफॉल्ट "User", "Admin" पाठवता येईल

    }
}