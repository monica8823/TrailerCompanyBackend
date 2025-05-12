using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrailerCompanyBackend.Models;

public partial class InventoryRecord//库存记录表，储存实际值。
{
    public int RecordId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public string OperationType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime OperationTime { get; set; }

    public string Operator { get; set; } = null!;

    public int? TargetStoreId { get; set; }

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Store? TargetStore { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
