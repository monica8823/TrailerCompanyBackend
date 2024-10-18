using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrailerCompanyBackend.Enums;


public class User
{
    // 自动递增的主键
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // 确保 UserId 是自动递增的
    public int UserId { get; set; }


    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;


    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;


    [Required]
    public string Role { get; set; } = "User";


    [Required]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

   
    [Required]
    public string Status { get; set; } = "Active";

    [NotMapped]
    public UserRole UserRoleEnum
    {
        get => Enum.Parse<UserRole>(Role);
        set => Role = value.ToString();
    }
}
