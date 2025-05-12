using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public class AccessorySizeRelation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ParentAccessorySize")]
        public int ParentAccessorySizeId { get; set; }
        public virtual AccessorySize ParentAccessorySize { get; set; } = null!;

        [ForeignKey("ChildAccessorySize")]
        public int ChildAccessorySizeId { get; set; }
        public virtual AccessorySize ChildAccessorySize { get; set; } = null!;

        public int Quantity { get; set; }  // 比如 主件-1 子件-4
    }
}
