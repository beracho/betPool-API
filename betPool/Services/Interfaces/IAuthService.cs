using budgetManager.Dto.UserDto;

namespace budgetManager.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserForDetailedDto> Register(UserForRegisterDto userForRegisterDto);
        Task<string> ValidateUsernameOrEmailExist(UserForRegisterDto userForRegisterDto);
    }
}
