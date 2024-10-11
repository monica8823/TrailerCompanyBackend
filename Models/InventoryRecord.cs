using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class InventoryRecord
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
