using budgetManager.Dto.UserDto;

namespace budgetManager.Dto.AuthDto
{
    public class AuthenticationDataDto
    {
        public string Token { get; set; } = default!;
        public UserForDetailedDto User { get; set; } = default!;
    }
}
