using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrailerCompanyBackend.Models;

public partial class RestockRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generated RestockId
    public int RestockId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    [Required]
    public DateTime RestockTime { get; set; }

    [Required]
    public int RestockQuantity { get; set; }

    [Required]
    public string Operator { get; set; } = null!;

    [Required]
    public string RestockMethod { get; set; } = null!; // Method of restocking (manual, transfer, etc.)

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
