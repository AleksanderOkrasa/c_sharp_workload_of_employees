using Microsoft.EntityFrameworkCore;

namespace WorkLoad
{
    public class EmployeeOps
    {
        public static async Task<IResult> getAllEmployees(DbInter db)
        {
            return Results.Ok(await db.Employees.ToListAsync());
        }

        public static async Task<IResult> insertEmployee(Employee employee, DbInter db)
        {
            db.Employees.Add(employee); 
            await db.SaveChangesAsync();

            return Results.Created($"/employees/{employee.Id}", employee);
        }
    }
}
