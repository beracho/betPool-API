using AutoMapper;
using budgetManager.Data;
using budgetManager.Dto.AuthDto;
using budgetManager.Dto.UserDto;
using budgetManager.Models;
using budgetManager.Repositories;
using budgetManager.Repositories.Interfaces;
using budgetManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<AuthenticationDataDto> AuthenticationData(string usernameOrEmail)
        {
            var userFromRepo = await _userRepo.GetUserByUsernameOrEmail(usernameOrEmail);

            //var apps = await _userPermissionRepo.GetPermissionsPerUser(userId, "Active");
            //var permissionssAssignedToList = _mapper.Map<List<AppsToListDto>>(apps);

            var tokenDescriptor = CreateToken(userFromRepo);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var detailedUser = _mapper.Map<UserForDetailedDto>(userFromRepo);

            var dataForAuthenticationDto = new AuthenticationDataDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = detailedUser
            };

            return dataForAuthenticationDto;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private SecurityTokenDescriptor? CreateToken(User userToReturn)
        {
            try
            {
                var claims = new[] {
                    new Claim (ClaimTypes.NameIdentifier, userToReturn.Id.ToString()),
                    new Claim (ClaimTypes.Name, userToReturn.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_config["AppSettings:Token"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenDescriptor;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ValidateEmailExist(string emailToValidate)
        {
            if (await _userRepo.EmailExists(emailToValidate))
            {
                return true;
            }

            return false;
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            usernameOrEmail = usernameOrEmail.Trim();

            var userFromRepo = await _userRepo.GetUserByUsernameOrEmail(usernameOrEmail);

            return userFromRepo;
        }

        public async Task<bool> Login(string usernameOrEmail, string password)
        {
            bool loggedIn = false;
            password = password.Trim();
            var user = await _userRepo.GetUserByUsernameOrEmail(usernameOrEmail);

            if (user != null && verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                loggedIn = true;

            if (loggedIn)
                await LoginAttemptSucceded(user);
            else
                await LoginAttemptFailed(user);

            return loggedIn;
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
                    userToCreate.Status = "Active";
                    userToCreate.RecoveryKey = "";

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

        private async Task LoginAttemptSucceded(User? user)
        {
            if (user != null)
            {
                user.LastActive = DateTime.Now;
                user.LoginAttempts = 0;
                await _userRepo.UpdateUser(user);
            }
        }

        private async Task LoginAttemptFailed(User? user)
        {
            if (user != null)
            {
                user.LoginAttempts = user.LoginAttempts + 1;
                if (user.LoginAttempts >= 3)
                    user.AttemptsUnblockTime = DateTime.Now.AddMinutes(1);
                await _userRepo.UpdateUser(user);
            }
        }

        public async Task<LoginValidation> ValidateAccountForLogin(string usernameOrEmail)
        {
            var userFromRepo = await GetUserByUsernameOrEmail(usernameOrEmail);
            LoginValidation loginValidation = new LoginValidation()
            {
                Message = "Success",
                AttemptsLeft = 3 - userFromRepo.LoginAttempts
            };
            if (userFromRepo.Id == new Guid())
            {
                loginValidation.Message = "Nor user or email exist";
            }
            else if (loginValidation.AttemptsLeft == 0 && userFromRepo.AttemptsUnblockTime > DateTime.Now)
            {
                loginValidation.Message = "3 attempts limit has been reached, wait 1 minute to retry.";
            }

            return loginValidation;
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

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }
    }
}
