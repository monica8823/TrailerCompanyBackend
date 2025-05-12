using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




public class InvalidToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = null!; // 被标记为无效的 Token

    [Required]
    public DateTime ExpiredAt { get; set; } // Token 的过期时间
}
