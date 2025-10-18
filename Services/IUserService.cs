using UserApi.Models;
using UserApi.DTOs;

namespace UserApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserCreateDto userDto);
        Task<User?> UpdateUserAsync(int id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}