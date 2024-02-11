using System.ComponentModel.DataAnnotations;

namespace budgetManager.Dto.AuthDto
{
    public class UserForLoginDto
    {
        [Required]
        public string UsernameOrEmail { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
