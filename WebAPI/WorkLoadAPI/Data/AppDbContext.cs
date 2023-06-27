using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using WorkLoadAPI.Models;

namespace WorkLoadAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Duty> Duties => Set<Duty>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            modelBuilder.Entity<Employee>(e =>
            {
                e.HasData(new Employee
                {
                    Id = 1,
                    FirstName = "Pawel",
                    LastName = "Kownacki",
                });
                e.HasData(new Employee
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Testowy",
                });
            });

            modelBuilder.Entity<Duty>(d =>
            {
                d.HasData(new Duty
                {
                    Id = 1,
                    DutyDescription = "Test2137",
                    Time = 3,
                    Priority = 1,
                });
                d.HasData(new Duty
                {
                    Id = 2,
                    DutyDescription = "Test420",
                    Time = 5,
                    Priority = 5,
                    EmployeeId = 1,
                });
            });
        }
    }
}
