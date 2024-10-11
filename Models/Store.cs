using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string StoreAddress { get; set; } = null!;

    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

    public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();

    public virtual ICollection<Trailer> Trailers { get; set; } = new List<Trailer>();

    public virtual ICollection<TransferRecord> TransferRecordSourceStores { get; set; } = new List<TransferRecord>();

    public virtual ICollection<TransferRecord> TransferRecordTargetStores { get; set; } = new List<TransferRecord>();
}
