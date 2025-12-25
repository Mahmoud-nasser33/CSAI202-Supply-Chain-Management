// Manages HTTP requests and API logic for Auth.
using Microsoft.AspNetCore.Mvc;
using SupplyChain.Backend.Models;
using SupplyChain.Backend.Repositories;

namespace SupplyChain.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Email and password are required" });
                }

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var isValid = await _userRepository.ValidateUserAsync(request.Email, request.Password);
                if (!isValid)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var role = MapRoleToFrontend(user.Role_Name);

                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = user.UserID,
                        name = user.User_Name,
                        email = user.Email,
                        role = role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during login", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Email and password are required" });
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
                    RoleID = 2
                };

                var createdUser = await _userRepository.CreateUserAsync(user);

                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = createdUser.UserID,
                        name = createdUser.User_Name,
                        email = createdUser.Email,
                        role = "customer"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during registration", error = ex.Message });
            }
        }

        private string MapRoleToFrontend(string? roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "admin",
                "customer" => "customer",
                "supplier" => "supplier",
                "warehouse manager" => "user",
                _ => "customer"
            };
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
