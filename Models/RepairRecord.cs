using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class RepairRecord
{
    public int RepairId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime RepairTime { get; set; }

    public string RepairDetails { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
