using System.ComponentModel.DataAnnotations;

namespace budgetManager.Dto.UserDto
{
    public class UserForRegisterDto
    {
        //Front Data 
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Must have a password between 4 and 32 characters")]
        public string Password { get; set; } = default!;
        [Required]
        public string Email { get; set; } = default!;
        public string Telephone { get; set; } = default!;
        public string Status { get; set; } = default!;
        public UserForRegisterDto()
        {
            Status = "Active";
        }
    }
}
