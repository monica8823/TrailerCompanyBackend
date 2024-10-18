using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TrailerCompanyBackend.Models;

public partial class AlertRecord
{
    public int AlertId { get; set; }

    public int? TrailerId { get; set; }

    public int? AccessorySizeId { get; set; }

    public int CurrentQuantity { get; set; }

    public int ThresholdQuantity { get; set; }

    public DateTime AlertTime { get; set; }

    public string AlertType { get; set; } = null!;

    public virtual AccessorySize? AccessorySize { get; set; }

    public virtual Trailer? Trailer { get; set; }

    public string? Message { get; set; }
}
