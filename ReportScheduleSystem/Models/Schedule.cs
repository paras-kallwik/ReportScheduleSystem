using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReportScheduleSystem.Models
{
    public class Schedule
    {
        [Key]  // Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required]
        public int ReportId { get; set; }  // ✅ Foreign Key referencing Reports.Id

        [Required]
        public int User_Id { get; set; }  // ✅ Stores which user scheduled the report

        [Required]
        [StringLength(255)]
        public string Name { get; set; }  // Copy from Reports

        [StringLength(1000)]
        public string Description { get; set; } // Copy from Reports

        [Required]
        public bool Is_Active { get; set; } = true;

        [Required]
        public string CronExpression { get; set; }  // Defines scheduling time

        [Required]
        [EmailAddress]
        public string Email { get; set; }  // Email to send report
        [Required]
        public DateTime ScheduledDateTime { get; set; }

        // ✅ Navigation property to link with Reports
        [ForeignKey("ReportId")]
        public Reports Report { get; set; }
    }

}
