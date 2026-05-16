using System.ComponentModel.DataAnnotations;



    namespace Personal_Blogging_Platform.Data.DTOs
    {
        public class VerifyEmailDto
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 characters long.")]
            public string Otp { get; set; }
        }
    }

