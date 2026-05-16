using System.ComponentModel.DataAnnotations;

namespace Personal_Blogging_Platform.Data.DTOs.auth
{
    public class UserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}
