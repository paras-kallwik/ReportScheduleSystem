using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReportScheduleSystem.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool Is_Active { get; set; } = true;

        [Required]
        public string CronExpression { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime ScheduledDateTime { get; set; }

        // ✅ New: Add frequency (Daily / Weekly / Monthly)
        [Required]
        [Display(Name = "Send Frequency")]
        public string Frequency { get; set; }

        [ForeignKey("ReportId")]
        public Reports Report { get; set; }
    }
}
