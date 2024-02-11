using budgetManager.Dto.UserDto;
using budgetManager.Models;

namespace budgetManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserForDetailedDto> GetUserById(string UserId);
    }
}
