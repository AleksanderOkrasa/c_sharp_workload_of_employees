using Microsoft.EntityFrameworkCore;

namespace WorkLoad
{
    public class DbInter: DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbInter(DbContextOptions<DbInter> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(new Employee() { Id = 1, firstName = "Pawel", lastName = "Kownacki" });
        }
    }
}
