using TrailerCompanyBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TrailerCompanyBackend.Services
{
    public class UserService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(TrailerCompanyDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> RegisterUserAsync(string email, string password, UserRole role)
        {
            try
            {
                if (!email.EndsWith("@kingkongtrailers.com.au"))
                {
                    _logger.LogWarning("Registration attempt with non-company email: {Email}", email);
                    return null;
                }

                if (await _context.Users.AnyAsync(u => u.Email == email))
                {
                    _logger.LogWarning("Registration attempt with already registered email: {Email}", email);
                    return null;
                }

                string hashedPassword = HashPassword(password);

                var newUser = new User
                {
                    Email = email,
                    Password = hashedPassword,
                    Role = role.ToString(),
                    RegistrationDate = DateTime.Now,
                    Status = "Active"
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully with email: {Email}", email);
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user with email: {Email}", email);
                throw;
            }
        }

        public async Task<User?> LoginUserAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == "Active");

                if (user == null)
                {
                    _logger.LogWarning("Login attempt failed for email: {Email}", email);
                    return null;
                }

                if (!VerifyPassword(password, user.Password))
                {
                    _logger.LogWarning("Login attempt with invalid password for email: {Email}", email);
                    return null;
                }

                _logger.LogInformation("User logged in successfully with email: {Email}", email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user with email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("Attempt to delete non-existing user with ID: {UserId}", userId);
                    return false;
                }

                user.Status = "Inactive";
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID: {UserId} was deactivated.", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                _logger.LogInformation("All users retrieved successfully.");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users.");
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    _logger.LogWarning("Attempt to retrieve non-existing user with ID: {UserId}", userId);
                    return null;
                }

                _logger.LogInformation("User with ID: {UserId} retrieved successfully.", userId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Attempt to change password for non-existing user with ID: {UserId}", userId);
                    return false;
                }

                user.Password = HashPassword(newPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Password changed successfully for user with ID: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password for user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("Attempt to reset password for non-existing user with email: {Email}", email);
                    return false;
                }

                user.Password = HashPassword(newPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Password reset successfully for user with email: {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password for user with email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> AssignRoleAsync(int adminId, int userId, string newRole)
        {
            try
            {
                var admin = await _context.Users.FindAsync(adminId);
                if (admin == null || admin.Role != "admin")
                {
                    _logger.LogWarning("AssignRole attempt by non-admin user with ID: {AdminId}", adminId);
                    return false;
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("Attempt to assign role to non-existing user with ID: {UserId}", userId);
                    return false;
                }

                var validRoles = new List<string> { "user", "manager", "admin" };
                if (!validRoles.Contains(newRole.ToLower()))
                {
                    _logger.LogWarning("Attempt to assign invalid role: {NewRole}", newRole);
                    return false;
                }

                user.Role = newRole;
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Role {NewRole} assigned to user with ID: {UserId} by admin with ID: {AdminId}", newRole, userId, adminId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning role {NewRole} to user with ID: {UserId} by admin with ID: {AdminId}", newRole, userId, adminId);
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string hashedPassword = HashPassword(enteredPassword);
            return hashedPassword == storedHashedPassword;
        }
    }
}
