using System;
using System.ComponentModel.DataAnnotations;

namespace TrailerCompanyBackend.Models
{
    public class BackupRecord
    {
        [Key]
        public int BackupRecordId { get; set; }

        public DateTime BackupTime { get; set; }

        public string FileName { get; set; } = null!;
    }
}
