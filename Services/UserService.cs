using UserApi.Models;
using UserApi.DTOs;
using System.Collections.Concurrent;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private static readonly ConcurrentDictionary<int, User> _users = new();
        private static int _nextId = 1;

        static UserService()
        {
            // Add some sample data
            _users[1] = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "+1-555-0123",
                Age = 30,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            };

            _users[2] = new User
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "+1-555-0124",
                Age = 28,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            };

            _nextId = 3;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            await Task.Delay(1); // Simulate async operation
            return _users.Values.OrderBy(u => u.Id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            _users.TryGetValue(id, out var user);
            return user;
        }

        public async Task<User> CreateUserAsync(UserCreateDto userDto)
        {
            await Task.Delay(1); // Simulate async operation

            var user = new User
            {
                Id = Interlocked.Increment(ref _nextId) - 1,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Age = userDto.Age,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _users[user.Id] = user;
            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            await Task.Delay(1); // Simulate async operation

            if (!_users.TryGetValue(id, out var existingUser))
                return null;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.Age = userDto.Age;
            existingUser.UpdatedAt = DateTime.UtcNow;

            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.TryRemove(id, out _);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.ContainsKey(id);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.Values.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                                         (excludeUserId == null || u.Id != excludeUserId));
        }
    }
}