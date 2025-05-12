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

    [Required]
    public string StoreName { get; set; } = null!; // 门店名称

    // 配件关系
    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

    // 库存记录关系
    public virtual ICollection<InventoryRecord> InventoryRecords { get; set; } = new List<InventoryRecord>();

    // 转移记录关系（源门店）
    public virtual ICollection<TransferRecord> TransferRecordSourceStores { get; set; } = new List<TransferRecord>();

    // 转移记录关系（目标门店）
    public virtual ICollection<TransferRecord> TransferRecordTargetStores { get; set; } = new List<TransferRecord>();

    // 拖车模型关系
    public virtual ICollection<TrailerModel> TrailerModels { get; set; } = new List<TrailerModel>();

    // 月度记录关系
    public virtual ICollection<Month> Months { get; set; } = new List<Month>();

    // 新增：集装箱入库记录关系
    public virtual ICollection<ContainerEntryRecord> ContainerEntryRecords { get; set; } = new List<ContainerEntryRecord>();
}
