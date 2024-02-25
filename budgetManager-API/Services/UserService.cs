using AutoMapper;
using budgetManager.Data;
using budgetManager.Dto.UserDto;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;
using budgetManager.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

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

        public async Task UpdateUserPassword(string Email, string NewPassword)
        {
            var userFromRepo = await _userRepo.GetUserByUsernameOrEmail(Email);
            userFromRepo.RecoveryKey = "";
            userFromRepo.RecoveryDate = DateTime.Now;

            var password = NewPassword.Trim();
            byte[] passwordHash, passwordSalt;
            _userRepo.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            userFromRepo.PasswordHash = passwordHash;
            userFromRepo.PasswordSalt = passwordSalt;

            await _userRepo.UpdateUser(userFromRepo);
        }
    }
}
