using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class DisposalRecord
{
    public int DisposalId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime DisposalTime { get; set; }

    public string Reason { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
