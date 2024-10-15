public enum UserRole
{
    Admin,
    Manager,
    User
}

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
 
    public string Role { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = null!;

  
    public UserRole UserRoleEnum
    {
        get => Enum.Parse<UserRole>(Role); 
        set => Role = value.ToString(); 
    }
}
