using budgetManager.Data;
using budgetManager.Models;
using budgetManager.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace budgetManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public string GenerateRecoveryKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string key = new String(stringChars);

            return key;
        }

        public async Task<User> GetUserById(string userId)
        {
            Guid userGuid = new Guid(userId);
            var userFromRepo = await _context.Users.FirstOrDefaultAsync(x => x.Id == userGuid);
            return userFromRepo == null ? new User() : userFromRepo;
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            var userFromRepo = await _context.Users.FirstOrDefaultAsync(x => (x.Username == usernameOrEmail || x.Email == usernameOrEmail) && x.Status == "Active");

            return userFromRepo == null ? new User() : userFromRepo;
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
