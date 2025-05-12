using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public class ContainerStagingAccessoryRecord
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ContainerEntryRecord")]
    public int ContainerEntryId { get; set; }
    public virtual ContainerEntryRecord ContainerEntryRecord { get; set; } = null!;

    [ForeignKey("Accessory")]
    public int AccessoryId { get; set; }
    public virtual Accessory Accessory { get; set; } = null!;

    [ForeignKey("AccessorySize")]
    public int AccessorySizeId { get; set; }
    public virtual AccessorySize AccessorySize { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    public string? Remarks { get; set; }
}

}