using Microsoft.EntityFrameworkCore;

namespace ReportScheduleSystem.Models
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext>option):base (option)
        {
                
        }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        
    }
}
