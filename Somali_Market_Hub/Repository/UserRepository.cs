// File: Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using Somali_Market_Hub.Data;
using Somali_Market_Hub.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Somali_Market_Hub.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SMHDbContext _context;

        public UserRepository(SMHDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(string id) => await _context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();

        public async Task AddUserAsync(User user)
        {
            user.Id = await GenerateUserIdAsync(user.Role);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateUserIdAsync(string role)
        {
            string prefix = role switch
            {
                "Admin" => "SA",
                "Provider" => "SP",
                "Customer" => "SC",
                _ => ""
            };

            var lastUser = await _context.Users
                .Where(u => u.Role == role)
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();

            int nextNumber = lastUser != null ? int.Parse(lastUser.Id.Substring(2)) + 1 : 1;
            return $"{prefix}{nextNumber:D2}";
        }

        public async Task<User> GetLastUserByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();
        }
    }
}