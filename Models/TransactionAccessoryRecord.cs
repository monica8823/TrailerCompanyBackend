using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models

{
   public class TransactionAccessoryRecord
{
    [Key]
    public int TransactionId { get; set; }  

    [Required]
    public string TransactionType { get; set; } = null!;
    // 📌 业务类型： "ContainerEntry"（入库）, "Sales"（销售）, "Repair"（维修）, "Transfer"（转移）, "Assembly"（组装）

    [ForeignKey("AccessorySize")]
    public int AccessorySizeId { get; set; }

    public virtual AccessorySize AccessorySize { get; set; } = null!;

    [ForeignKey("Trailer")]
    public int? TrailerId { get; set; }  // 📌 只有在 **组装/销售** 业务时才会填入具体拖车 ID
    public virtual Trailer? Trailer { get; set; }

    [Required]
    public int Quantity { get; set; }  // 记录配件数量

    [Required]
    public DateTime TransactionTime { get; set; } // 业务发生时间

    public string? Remarks { get; set; } // 备注信息
}

}