﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workload.Services;
using Workload.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Workload.Basic;

namespace Workload.ViewModel
{
    internal class EmployeeManagerViewModel : ViewModelBase
    {
        private ApiService _apiService;
        private WorkloadViewModel _workloadViewModel;
        public ObservableCollection<EmployeeModel> Employees { get; set; }
        private string firstName;
        private string lastName;

        public Command AddEmployeeCommand { get; }

        public EmployeeManagerViewModel(WorkloadViewModel workloadViewModel)
        {
            _workloadViewModel = workloadViewModel;
            _apiService = new ApiService("http://127.0.0.1:5052");
            DeleteEmployeeCommand = new RelayCommand<EmployeeModel>(DeleteEmployee);
            AccelerateEmployeeCommand = new RelayCommand<EmployeeModel>(AccelerateEmployee);
            AddEmployeeCommand = new Command(AddEmployee);
            Employees = workloadViewModel.Employees;
        }

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

        public ICommand DeleteEmployeeCommand { get; private set; }
        public ICommand AccelerateEmployeeCommand { get; private set; }

        public async Task AddEmployeeToDB(EmployeeModel employee)
        {
            await _apiService.PostEmployee(employee);
            var employeesFromApi = await _apiService.GetEmployees();
            await UpdateNewEmployeesCollection(employeesFromApi);
        }

        public int GenerateNewEmployeeID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }

        private async void DeleteEmployee(EmployeeModel employee)
        {
            await _apiService.DeleteEmployee(employee.Id);
            var EmployeesFromApi = await _apiService.GetEmployees();

            await UpdateRemovedEmployeesCollection(EmployeesFromApi);
            await _workloadViewModel.EditDutiesWithEmployeeId(employee.Id, "delete_employee");
        }

        private async void AccelerateEmployee(EmployeeModel employee)
        {
            await _workloadViewModel.EditDutiesWithEmployeeId(employee.Id, "accelerate");
        }


        public async Task LoadEmployeeDataFromApi()
        {
            var employeesFromApi = await _apiService.GetEmployees();
            await UpdateNewEmployeesCollection(employeesFromApi);
        }

        private async Task UpdateNewEmployeesCollection(ObservableCollection<EmployeeModel> EmployeesFromApi)
        {
            foreach (EmployeeModel employeeFromApi in EmployeesFromApi)
            {
                if (!Employees.Any(employee => employee.Id == employeeFromApi.Id))
                {
                    Employees.Add(employeeFromApi);
                }
            }
        }
        private async Task UpdateRemovedEmployeesCollection(ObservableCollection<EmployeeModel> EmployeesFromApi)
        {
            List<EmployeeModel> dutiesToRemove = new List<EmployeeModel>();

            foreach (EmployeeModel employeeInCollection in Employees)
            {
                if (!EmployeesFromApi.Any(employee => employee.Id == employeeInCollection.Id))
                {
                    dutiesToRemove.Add(employeeInCollection);
                }
            }

            if (dutiesToRemove.Any())
            {
                foreach (EmployeeModel employeeToRemove in dutiesToRemove)
                {
                    Employees.Remove(employeeToRemove);
                }
            }
        }
    }
}
