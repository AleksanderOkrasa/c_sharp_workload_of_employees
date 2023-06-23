using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Basic;
using WpfApp.Model;


namespace WpfApp.ViewModel
{
    public partial class EmployeeManagerViewModel : ViewModelBase
    {
        private string? firstName;
        private string? lastName;
        private WorkloadViewModel workloadViewModel;

      //  public EmployeeManagerViewModel()
     //   {
    //        AddEmployeeCommand = new Command(AddEmployee);
    //        workload = new WorkloadViewModel();
     //       Employees = workload.Employees;
        //}
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
        


        private void AddEmployee()
        {
            EmployeeModel newEmployee = new EmployeeModel
            {
                ID = GenerateNewEmployeeID(),
                FirstName = FirstName,
                LastName = LastName,
            };

            Employees.Add(newEmployee);
            //workloadViewModel.Employees.Add(newEmployee);
            FirstName = string.Empty;
            LastName = string.Empty;

        }

        private int GenerateNewEmployeeID()
        {
            if (Employees.Count > 0)
                return Employees[Employees.Count - 1].ID + 1;
            else
                return 1;
        }
    }
}
