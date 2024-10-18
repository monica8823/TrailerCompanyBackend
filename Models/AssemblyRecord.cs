using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrailerCompanyBackend.Models;

public partial class AssemblyRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AssemblyId { get; set; }

    public int TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public DateTime AssemblyTime { get; set; }

    public string Operator { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer Trailer { get; set; } = null!;
}
