using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Basic;
using Workload.Models;


namespace WpfApp.ViewModel
{
    public partial class EmployeeManagerViewModel : ViewModelBase
    {
        private string? firstName;
        private string? lastName;
        private WorkloadViewModel workloadViewModel;

        public EmployeeManagerViewModel(WorkloadViewModel workloadViewModel)
        {
            this.workloadViewModel = workloadViewModel;
            AddEmployeeCommand = new Command(AddEmployee);
            Employees = workloadViewModel.Employees;
        }
        public Command AddEmployeeCommand { get; private set; }

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        
        public string FirstName { get => firstName; set => Set(ref firstName, value); }
        public string LastName { get => lastName; set => Set(ref lastName, value); }
        


        private async void AddEmployee()
        {
            EmployeeModel newEmployee = new EmployeeModel
            {
                Id = GenerateNewEmployeeID(),
                FirstName = FirstName,
                LastName = LastName,
            };

            await AddEmployeeToDB(newEmployee);

            FirstName = string.Empty;
            LastName = string.Empty;

        }

        public virtual async Task AddEmployeeToDB(EmployeeModel employee)
        {
            Employees.Add(employee);
        }

        private int GenerateNewEmployeeID()
        {
            if (Employees.Count > 0)
                return Employees[Employees.Count - 1].Id + 1;
            else
                return 1;
        }
    }
}
