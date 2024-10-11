using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class Accessory
{
    public int AccessoryId { get; set; }

    public string AccessoryType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int StoreId { get; set; }

    public virtual ICollection<AccessorySize> AccessorySizes { get; set; } = new List<AccessorySize>();

    public virtual Store Store { get; set; } = null!;
}
