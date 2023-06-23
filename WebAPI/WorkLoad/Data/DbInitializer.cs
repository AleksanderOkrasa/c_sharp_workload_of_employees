using Microsoft.EntityFrameworkCore;
using WorkLoad.Models;

namespace WorkLoad.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder _builder;

        public DbInitializer(ModelBuilder builder)
        {
            _builder = builder;
        }

        public void Seed()
        {
            _builder.Entity<EmployeeModel>(e =>
            {
                e.HasData(new EmployeeModel
                {
                    Id = 1,
                    FirstName = "Pawel",
                    LastName = "Kownacki",
                });
                e.HasData(new EmployeeModel
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Testowy",
                });
            });

            _builder.Entity<TaskModel>(t => 
            { 
                t.HasData(new TaskModel 
                { 
                    Id = 3, 
                    Priorty = 5, 
                    TaskDescription = "Defekacja", 
                    Time = 0.5,
                    EmployeeId = 1,
                });
                t.HasData(new TaskModel
                {
                    Id = 2,
                    Priorty = 1,
                    TaskDescription = "Test",
                    Time = 2,
                });
            });
        }
    }
}
    

