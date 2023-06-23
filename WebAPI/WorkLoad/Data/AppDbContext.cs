using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WorkLoad.Models;

namespace WorkLoad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TaskModel>().HasOne(p => p.Employee).WithMany(p => p.Tasks);

            new DbInitializer(builder).Seed();
        }
           }
}
