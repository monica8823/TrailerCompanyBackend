using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models

{
public class TransactionTrailerRecord//关系表： 标识业务和拖车的关系，但不储存实际值。
{
    [Key]
    public int TransactionId { get; set; }  

    [Required]
    public string TransactionType { get; set; } = null!;
    // 📌 业务类型： "ContainerEntry"（入库）, "Sales"（销售）, "Repair"（维修）, "Transfer"（转移）, "Assembly"（组装）

    [ForeignKey("Trailer")]
    public int TrailerId { get; set; }  // 📌 只存一辆拖车！因为每辆拖车都有ID，所以只需要一个外键即可
    public virtual Trailer Trailer { get; set; } = null!;

    [Required]
    public DateTime TransactionTime { get; set; } 

    public string? Remarks { get; set; }
}
}