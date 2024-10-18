using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models;

public partial class SalesRecord
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime SalesTime { get; set; }

    public double SalesPrice { get; set; }

    public string InvNumber { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }
}
