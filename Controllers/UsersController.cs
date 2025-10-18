using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.DTOs;
using UserApi.Services;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation("Getting user with ID: {UserId}", id);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">User creation data</param>
        /// <returns>Created user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto userDto)
        {
            _logger.LogInformation("Creating new user with email: {Email}", userDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user creation");
                return BadRequest(ModelState);
            }

            // Check if email already exists
            if (await _userService.EmailExistsAsync(userDto.Email))
            {
                _logger.LogWarning("Email {Email} already exists", userDto.Email);
                return Conflict($"User with email {userDto.Email} already exists");
            }

            var user = await _userService.CreateUserAsync(userDto);
            _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userDto">User update data</param>
        /// <returns>Updated user</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user update");
                return BadRequest(ModelState);
            }

            // Check if user exists
            if (!await _userService.UserExistsAsync(id))
            {
                _logger.LogWarning("User with ID {UserId} not found for update", id);
                return NotFound($"User with ID {id} not found");
            }

            // Check if email already exists for another user
            if (await _userService.EmailExistsAsync(userDto.Email, id))
            {
                _logger.LogWarning("Email {Email} already exists for another user", userDto.Email);
                return Conflict($"Email {userDto.Email} is already in use by another user");
            }

            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            if (updatedUser == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            _logger.LogInformation("User with ID {UserId} updated successfully", id);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                return NotFound($"User with ID {id} not found");
            }

            _logger.LogInformation("User with ID {UserId} deleted successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Check if a user exists
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Boolean indicating if user exists</returns>
        [HttpHead("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UserExists(int id)
        {
            var exists = await _userService.UserExistsAsync(id);
            return exists ? Ok() : NotFound();
        }
    }
}