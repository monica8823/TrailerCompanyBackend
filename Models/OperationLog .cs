using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class OperationLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generated LogId
    public int LogId { get; set; }

    // The type of entity that was affected (e.g., Trailer, Accessory)
    [Required]
    public string EntityType { get; set; } = null!;

    // The ID of the affected entity (e.g., TrailerId, AccessoryId)
    [Required]
    public int EntityId { get; set; }

    // The type of operation (e.g., Created, Updated, Deleted)
    [Required]
    public string OperationType { get; set; } = null!;

    // The user who performed the operation
    [Required]
    public int UserId { get; set; }

    // Description or details of the operation
    [Required]
    public string Description { get; set; } = null!;

    // The timestamp of the operation
    [Required]
    public DateTime OperationTime { get; set; }
}
