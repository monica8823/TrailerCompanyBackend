using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public partial class TrailerAccessorySize //关系表，拖车与配件得关系，不储存实际值
    {
        [Key]
        public int TrailerAccessorySizeId { get; set; }

        [ForeignKey("Trailer")]
        public int TrailerId { get; set; }
        public virtual Trailer Trailer { get; set; } = null!;

        [ForeignKey("AccessorySize")]
        public int AccessorySizeId { get; set; }
        public virtual AccessorySize AccessorySize { get; set; } = null!;
        

        // 记录每种规格配件的数量
        public int Quantity { get; set; }
    }
}
