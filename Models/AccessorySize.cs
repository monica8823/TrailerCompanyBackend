using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrailerCompanyBackend.Models;

public partial class AccessorySize
{
    public int SizeId { get; set; }

    public int AccessoryId { get; set; }

    public string SizeName { get; set; } = null!;

    public string DetailedSpecification { get; set; } = null!;

    public int ThresholdQuantity { get; set; }

    public virtual Accessory Accessory { get; set; } = null!;

    public virtual ICollection<AlertRecord> AlertRecords { get; set; } = new List<AlertRecord>();

    public virtual ICollection<AssemblyRecord> AssemblyRecords { get; set; } = new List<AssemblyRecord>();

    public virtual ICollection<DisposalRecord> DisposalRecords { get; set; } = new List<DisposalRecord>();

    public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();

    public virtual ICollection<RepairRecord> RepairRecords { get; set; } = new List<RepairRecord>();

    public virtual ICollection<ContainerEntryRecord> ContainerEntryRecords { get; set; } = new List<ContainerEntryRecord>();

    public virtual ICollection<SalesRecord> SalesRecords { get; set; } = new List<SalesRecord>();

    public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();

    public virtual ICollection<TrailerAccessorySize> TrailerAccessorySize { get; set; } = new List<TrailerAccessorySize>();

    public virtual ICollection<Trailer> Trailers { get; set; } = new List<Trailer>();
}
