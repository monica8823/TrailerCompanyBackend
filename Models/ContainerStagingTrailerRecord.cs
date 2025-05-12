using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrailerCompanyBackend.Models
{
    public class ContainerStagingTrailerRecord
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ContainerEntryRecord")]
    public int ContainerEntryId { get; set; }
    public virtual ContainerEntryRecord ContainerEntryRecord { get; set; } = null!;

    public string? ModelName { get; set; }

    public string? Vin { get; set; }

    public string? Remarks { get; set; }
}



}