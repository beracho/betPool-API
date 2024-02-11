using AutoMapper;
using budgetManager.Data;
using budgetManager.Dto.UserDto;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;
using budgetManager.Services.Interfaces;

namespace budgetManager.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepo;
        public UserService(IMapper mapper, IConfiguration config, IUserRepository userRepo)
        {
            _mapper = mapper;
            _config = config;
            _userRepo = userRepo;
        }

        public async Task<UserForDetailedDto> GetUserById(string UserId)
        {
            var userFromRepo = await _userRepo.GetUserById(UserId);
            var userToReturn = _mapper.Map<UserForDetailedDto>(userFromRepo);

            return userToReturn;
        }
    }
}
