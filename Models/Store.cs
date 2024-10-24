using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models;

public partial class Store
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string StoreAddress { get; set; } = null!;

    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

    public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();

    public virtual ICollection<Trailer> Trailers { get; set; } = new List<Trailer>();

    public virtual ICollection<TransferRecord> TransferRecordSourceStores { get; set; } = new List<TransferRecord>();

    public virtual ICollection<TransferRecord> TransferRecordTargetStores { get; set; } = new List<TransferRecord>();

     public virtual ICollection<TrailerModel> TrailerModels { get; set; } = new List<TrailerModel>();
    
}
