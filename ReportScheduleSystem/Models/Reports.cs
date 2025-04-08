using System;
using System.ComponentModel.DataAnnotations;

namespace ReportScheduleSystem.Models
{
    public class Reports
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool Is_Active { get; set; } = true;

        [StringLength(255)]
       // [Required(ErrorMessage = "File is required.")]
        public string? FileName { get; set; } // Nullable if no file is uploaded

        [StringLength(100)]
        public string? ContentType { get; set; } // Nullable to avoid unnecessary storage
       // [Required(ErrorMessage = "File data is required.")]
        public byte[]? FileData { get; set; } // Nullable to allow reports without files


        public DateTime Created_At { get; set; } = DateTime.UtcNow; // Auto-set on creation

        
       
        public DateTime Updated_At { get; set; } = DateTime.UtcNow; // Auto-set on creation
        public ICollection<Schedule> Schedules { get; set; }
    }
}
