using Microsoft.EntityFrameworkCore;

namespace WorkLoad
{
    public class DbInter: DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbInter(DbContextOptions<DbInter> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasMany(e => e.tasks).WithOne(e => e.employee).HasForeignKey(e => e.employeeID).IsRequired(false);
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 1, firstName = "Pawel", lastName = "Kownacki" });
            modelBuilder.Entity<Task>().HasData(new Task() { Id= 1, description = "Defekacja", priority = 5, time = 1, isDone = false });
        }
    }
}
