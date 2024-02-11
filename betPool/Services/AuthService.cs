using AutoMapper;
using budgetManager.Data;
using budgetManager.Dto.UserDto;
using budgetManager.Models;
using budgetManager.Repositories;
using budgetManager.Repositories.Interfaces;
using budgetManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace budgetManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepo;
        public AuthService(DataContext context, IMapper mapper, IConfiguration config, IUserRepository userRepo)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _userRepo = userRepo;
        }

        public async Task<UserForDetailedDto> Register(UserForRegisterDto userForRegisterDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var password = userForRegisterDto.Password.Trim();

                    var userToCreate = _mapper.Map<User>(userForRegisterDto);

                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);
                    userToCreate.PasswordHash = passwordHash;
                    userToCreate.PasswordSalt = passwordSalt;
                    userToCreate.Status = "Activ";

                    var createdUser = await _userRepo.CreateUser(userToCreate);
                    var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

                    transaction.Commit();

                    return userToReturn;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<string> ValidateUsernameOrEmailExist(UserForRegisterDto userForRegisterDto)
        {
            if (await _userRepo.UsernameExists(userForRegisterDto.Username))
            {
                return "User '" + userForRegisterDto.Username + "' already exists.";
            }
            if (await _userRepo.EmailExists(userForRegisterDto.Email))
            {
                return "The email '" + userForRegisterDto.Email + "' has already been registered.";
            }

            return "";
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
