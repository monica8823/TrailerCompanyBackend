using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public partial class ContainerEntryRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContainerEntryId { get; set; }

    [Required]
    public string ContainerNumber { get; set; } = null!; // 集装箱号，唯一

    [Required]
    public DateTime EntryTime { get; set; } // 预计到货 or 实际到货时间

    [Required]
    public int StoreId { get; set; } // 所属门店

    [Required]
    public string EntryStatus { get; set; } = "Pending"; 
    // Pending: 预入库
    // Confirmed: 实际入库完成

    public string? Remarks { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual ICollection<ContainerStagingTrailerRecord> Trailers { get; set; } = new List<ContainerStagingTrailerRecord>();

    public virtual ICollection<ContainerStagingAccessoryRecord> Accessories { get; set; } = new List<ContainerStagingAccessoryRecord>();


}
}