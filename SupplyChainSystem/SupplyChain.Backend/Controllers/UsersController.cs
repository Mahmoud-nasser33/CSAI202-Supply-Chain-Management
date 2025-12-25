// Manages HTTP requests and API logic for Users.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<User> users = await _userRepository.GetAllUsersAsync();

                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.UserID,
                    Name = u.User_Name,
                    Email = u.Email,
                    RoleId = u.RoleID,
                    RoleName = u.Role_Name
                }).ToList();
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = $"User with ID {id} not found" });

                var userDto = new UserDto
                {
                    Id = user.UserID,
                    Name = user.User_Name,
                    Email = user.Email,
                    RoleId = user.RoleID,
                    RoleName = user.Role_Name
                };
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return Conflict(new { message = "User with this email already exists" });
                }

                var user = new User
                {
                    User_Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    RoleID = request.RoleId
                };

                var created = await _userRepository.CreateUserAsync(user);
                var userDto = new UserDto
                {
                    Id = created.UserID,
                    Name = created.User_Name,
                    Email = created.Email,
                    RoleId = created.RoleID
                };
                return CreatedAtAction(nameof(GetUser), new { id = created.UserID }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    User_Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    RoleID = request.RoleId
                };

                bool updated = await _userRepository.UpdateUserAsync(id, user);
                if (!updated)
                    return NotFound(new { message = $"User with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating user", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool deleted = await _userRepository.DeleteUserAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"User with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting user", error = ex.Message });
            }
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }
}
