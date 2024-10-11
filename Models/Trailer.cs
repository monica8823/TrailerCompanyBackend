using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class Trailer
{
    public int TrailerId { get; set; }

    public string Vin { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string Size { get; set; } = null!;

    public double RatedCapacity { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public int StoreId { get; set; }

    public int ThresholdQuantity { get; set; }

    public virtual ICollection<AlertRecord> AlertRecords { get; set; } = new List<AlertRecord>();

    public virtual ICollection<AssemblyRecord> AssemblyRecords { get; set; } = new List<AssemblyRecord>();

    public virtual ICollection<DisposalRecord> DisposalRecords { get; set; } = new List<DisposalRecord>();

    public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();

    public virtual ICollection<RepairRecord> RepairRecords { get; set; } = new List<RepairRecord>();

    public virtual ICollection<RestockRecord> RestockRecords { get; set; } = new List<RestockRecord>();

    public virtual ICollection<SalesRecord> SalesRecords { get; set; } = new List<SalesRecord>();

    public virtual Store Store { get; set; } = null!;

    public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();

    public virtual ICollection<AccessorySize> AccessorySizes { get; set; } = new List<AccessorySize>();
}
