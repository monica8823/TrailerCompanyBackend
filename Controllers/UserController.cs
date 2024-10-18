using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Services;
using TrailerCompanyBackend.DTOs; 


namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _userService.RegisterUserAsync(registerRequest.Email, registerRequest.Password);

            if (newUser == null)
            {
                return BadRequest("Registration failed. Email might already be registered or invalid.");
            }

            return Ok(newUser);
}



        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.LoginUserAsync(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Login failed. Invalid email or password.");
            }

            return Ok(user); 
        }

        // DELETE: api/User/Delete/{userId}
        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var success = await _userService.DeleteUserAsync(userId);

            if (!success)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok($"User with ID {userId} deleted successfully.");
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/User/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok(user);
        }

        // PUT: api/User/ChangePassword/{userId}
        [HttpPut("ChangePassword/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userService.ChangePasswordAsync(userId, request.NewPassword);

            if (!success)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok($"Password changed successfully for user with ID {userId}.");
        }

        // PUT: api/User/ResetPassword
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userService.ResetPasswordAsync(request.Email, request.NewPassword);

            if (!success)
            {
                return NotFound($"User with email {request.Email} not found.");
            }

            return Ok("Password reset successfully.");
        }

        // PUT: api/User/AssignRole
        [HttpPut("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _userService.AssignRoleAsync(request.AdminId, request.UserId, request.NewRole);

            if (!success)
            {
                return BadRequest("Role assignment failed. Please check if the admin or user exist, or if the role is valid.");
            }

            return Ok($"Role {request.NewRole} assigned successfully to user with ID {request.UserId}.");
        }
    }
}
