using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class TransferRecord
{
    public int TransferId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime TransferTime { get; set; }

    public int SourceStoreId { get; set; }

    public int TargetStoreId { get; set; }

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Store SourceStore { get; set; } = null!;

    public virtual Store TargetStore { get; set; } = null!;

    public virtual Trailer? Trailer { get; set; }
}
