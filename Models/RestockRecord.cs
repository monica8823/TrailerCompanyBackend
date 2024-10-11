using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class RestockRecord
{
    public int RestockId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime RestockTime { get; set; }

    public int RestockQuantity { get; set; }

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
