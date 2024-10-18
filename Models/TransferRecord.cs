using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models;

public partial class TransferRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int TransferId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime TransferTime { get; set; }

    // 外键指向来源 Store
    [ForeignKey("SourceStore")]
    public int SourceStoreId { get; set; }
    public virtual Store SourceStore { get; set; } = null!;

    // 外键指向目标 Store
    [ForeignKey("TargetStore")]
    public int TargetStoreId { get; set; }
    public virtual Store TargetStore { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
