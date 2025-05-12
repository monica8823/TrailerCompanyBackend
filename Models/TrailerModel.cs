using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public class TrailerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrailerModelId { get; set; }

        public string ModelName { get; set; } = null!;

        [ForeignKey("Store")]
        [Required]
        public int StoreId { get; set; }

        [ForeignKey("Month")]
         public int? MonthId { get; set; }


        public virtual Store? Store { get; set; }


        public virtual ICollection<Trailer> Trailers { get; set; } = new List<Trailer>();
    }
}
