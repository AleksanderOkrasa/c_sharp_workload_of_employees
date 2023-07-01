using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workload.Services;
using Workload.Models;

namespace Workload.ViewModel
{
    internal class EmployeeManagerViewModel : WpfApp.ViewModel.EmployeeManagerViewModel
    {
        private ApiService _apiService;

        public EmployeeManagerViewModel(WpfApp.ViewModel.WorkloadViewModel workloadViewModel) : base(workloadViewModel)
        {
            _apiService = new ApiService("http://127.0.0.1:5052");
        }

        public override async Task AddEmployeeToDB(EmployeeModel employee)
        {
            Employees.Add(employee);
            await _apiService.PostEmployee(employee);
        }

        public override int GenerateNewEmployeeID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }
        }

}
