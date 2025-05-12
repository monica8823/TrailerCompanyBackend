using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 自动递增主键
    public int TokenId { get; set; }

    [Required]
    [MaxLength(500)] // 假设最大长度是500
    public string Token { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime ExpiresAt { get; set; }

    [Required]
    public int UserId { get; set; } // 外键

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

  

}
