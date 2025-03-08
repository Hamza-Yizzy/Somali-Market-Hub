using Somali_Market_Hub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Somali_Market_Hub.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<string> GenerateUserIdAsync(string role);

        Task<User> GetLastUserByRoleAsync(string role);
    }
}
