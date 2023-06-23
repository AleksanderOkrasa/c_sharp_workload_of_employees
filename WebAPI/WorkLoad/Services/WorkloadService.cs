using WorkLoad.Data;
using WorkLoad.Models;
using Microsoft.EntityFrameworkCore;
namespace WorkLoad.Services
{
    public class WorkloadService : IWorkloadService
    {
        private readonly AppDbContext _db;

        public WorkloadService(AppDbContext db)
        {
            _db = db;
        }

        #region Employees

        public async Task<List<EmployeeModel>> GetEmployeesAsync()
        {
            try
            {
                return await _db.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<EmployeeModel> GetEmployeeAsync(int id, bool includeTasks)
        {
            try
            {
                if (includeTasks)
                {
                    return await _db.Employees.Include(t => t.Tasks).FirstOrDefaultAsync(i => i.Id == id);
                }

                return await _db.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                await _db.Employees.AddAsync(employee);
                await _db.SaveChangesAsync();
                return await _db.Employees.FindAsync(employee.Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                _db.Entry(employee).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteEmployeeAsync(EmployeeModel employee)
        {
            try
            {
                var dbEmployee = await _db.Employees.FindAsync(employee.Id);

                if (dbEmployee != null)
                {
                    return (false, "Employee could not be found");
                }

                _db.Employees.Remove(employee);
                await _db.SaveChangesAsync();

                return (true, "Employee got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        #endregion Employee
        #region Tasks

        public async Task<List<TaskModel>> GetTasksAsync()
        {
            try
            {
                return await _db.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TaskModel> GetTaskAsync(int id)
        {
            try
            {
                return await _db.Tasks.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TaskModel> AddTaskAsync(TaskModel task)
        {
            try
            {
                await _db.Tasks.AddAsync(task);
                await _db.SaveChangesAsync();
                return await _db.Tasks.FindAsync(task.Id);
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
        {
            try
            {
                _db.Entry(task).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<(bool, string)> DeleteTaskAsync(TaskModel task)
        {
            try
            {
                var dbTask = await _db.Tasks.FindAsync(task.Id);

                if (dbTask != null) 
                {
                    return (false, "Taks could not be found");
                }

                _db.Remove(task);
                await _db.SaveChangesAsync();

                return (true, "Task got deleted");
            }
            catch(Exception ex)
            {
               return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        #endregion Tasks
    }
}
