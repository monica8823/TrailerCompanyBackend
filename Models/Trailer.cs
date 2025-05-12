using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrailerCompanyBackend.Enums;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Models
{
    public partial class Trailer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int TrailerId { get; set; }
        
        // 可选，防止出现有拖车但是 VIN 还没申请下来的情况
        public string? Vin { get; set; } = null!;

        [Required]
        public string CurrentStatus { get; set; } = null!; // 必须字段，用于区分拖车状态

        // Foreign key to TrailerModel
        [Required] // 明确指定为必填字段

        public int TrailerModelId { get; set; }

        public virtual TrailerModel TrailerModel { get; set; } = null!;

        // Custom fields in JSON format，可选
        public string? CustomFields { get; set; }

        public virtual ICollection<AlertRecord> AlertRecords { get; set; } = new List<AlertRecord>();
        public virtual ICollection<AssemblyRecord> AssemblyRecords { get; set; } = new List<AssemblyRecord>();
        public virtual ICollection<DisposalRecord> DisposalRecords { get; set; } = new List<DisposalRecord>();
        public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();
        public virtual ICollection<RepairRecord> RepairRecords { get; set; } = new List<RepairRecord>();
        public virtual ICollection<ContainerEntryRecord> ContainerEntryRecords { get; set; } = new List<ContainerEntryRecord>();
        public virtual ICollection<SalesRecord> SalesRecords { get; set; } = new List<SalesRecord>();
        public virtual ICollection<TransferRecord> TransferRecords { get; set; } = new List<TransferRecord>();
        public virtual ICollection<TrailerAccessorySize> TrailerAccessorySizes { get; set; } = new List<TrailerAccessorySize>();
        public virtual ICollection<AccessorySize> AccessorySizes { get; set; } = new List<AccessorySize>();
    }
}
