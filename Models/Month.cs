using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public class Month
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonthId { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,2}/\d{2}$", ErrorMessage = "MonthName must be in format MM/YY.")]
        public string MonthName { get; set; } = null!; // Format: MM/YY

        [ForeignKey("Store")]
        public int? StoreId { get; set; }

        public virtual Store? Store { get; set; }

        public virtual ICollection<TrailerModel> TrailerModels { get; set; } = new List<TrailerModel>();
    }
}
