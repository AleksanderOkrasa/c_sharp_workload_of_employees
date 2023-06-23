using WorkLoad.Models;

namespace WorkLoad.Services
{
    public interface IWorkloadService
    {
        //Employee Services
        Task<List<EmployeeModel>> GetEmployeesAsync(); //GET all employees
        Task<EmployeeModel> GetEmployeeAsync(int id, bool includeTasks = false); //GET single employee
        Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee); //POST new employee
        Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee); //PUT employee
        Task<(bool, string)> DeleteEmployeeAsync(EmployeeModel employee); // Delete employee

        //Task Services
        Task<List<TaskModel>> GetTasksAsync(); //GET all tasks
        Task<TaskModel> GetTaskAsync(int id); //GET single task
        Task<TaskModel> AddTaskAsync(TaskModel task); // POST new task
        Task<TaskModel> UpdateTaskAsync(TaskModel task); // PUT task
        Task<(bool, string)> DeleteTaskAsync(TaskModel task); // DELETE task
    }

}