namespace TrailerCompanyBackend.DTOs
{
    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }


    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string? NewPassword { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
    }

    public class AssignRoleRequest
    {
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string? NewRole { get; set; }
    }
}
