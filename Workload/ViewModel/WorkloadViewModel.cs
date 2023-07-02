using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Workload.Models;
using Workload.Services;
using WpfApp.Basic;

namespace Workload.ViewModel
{
    internal class WorkloadViewModel : WpfApp.ViewModel.WorkloadViewModel
    {
        private ApiService _apiService;
        private DutyModel editedDuty;
        public WorkloadViewModel() : base()
        {
            _apiService = new ApiService("http://127.0.0.1:5052");
            DeleteDutyCommand = new RelayCommand<DutyModel>(DeleteDuty);
            LoadDataFromApi();
        }
        public ICommand DeleteDutyCommand { get; private set; }
        public DutyModel EditedDuty { get => editedDuty; set => Set(ref editedDuty, value); }

        public override async Task AddDutyToDB(DutyModel duty)
        {
            await _apiService.PostDuty(duty);

            var dutiesFromApi = await _apiService.GetDuties();
            await UpdateNewDutiesCollection(dutiesFromApi);
        }

        public override int GenerateNewDutyID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }

        private async void DeleteDuty(DutyModel duty)
        {
            await _apiService.DeleteDuty(duty.Id);
            var dutiesFromApi = await _apiService.GetDuties();

            await UpdateRemovedDutiesCollection(dutiesFromApi);
        }


        public async Task UpdateDuty(DutyModel dutyToUpdate)
        {
            await _apiService.UpdateDuty(dutyToUpdate);
            var oldDuty = Duties.FirstOrDefault(item => item.Id == dutyToUpdate.Id);
            if (oldDuty != null) 
            {
                oldDuty.DutyDescription = dutyToUpdate.DutyDescription;
                oldDuty.Priority = dutyToUpdate.Priority;
                oldDuty.Time = dutyToUpdate.Time;
                oldDuty.EmployeeId = dutyToUpdate.EmployeeId;
                RefreshDutiesViews();
            }

        }
        public async Task EditDutiesWithEmployeeId(int employeeId, string action)
        {
            foreach(DutyModel duty in Duties)
            {
                if (duty.EmployeeId == employeeId)
                {
                    if (action == "delete_employee")
                    {
                        duty.EmployeeId = 0;
                    }
                    if (action == "accelerate")
                    {
                        duty.Time = duty.Time * 0.9;
                    }
                    await UpdateDuty(duty);
                }
            }  
        }
        
        public double ReturnTotalTimeRequiredForEmployee(int employeeId)
        {
            double total = 0;
            foreach (DutyModel duty in Duties)
            {
                if (duty.EmployeeId == employeeId)
                {
                    total += duty.Time;
                }
            }
            return total;
        }

        private async Task LoadDataFromApi()
        {
            var dutiesFromApi = await _apiService.GetDuties();
            await UpdateNewDutiesCollection(dutiesFromApi);
        }

        private async Task UpdateNewDutiesCollection(ObservableCollection<DutyModel> DutiesFromApi)
        {
            foreach (DutyModel dutyFromApi in DutiesFromApi)
            {
                if (!Duties.Any(duty => duty.Id == dutyFromApi.Id))
                {
                    Duties.Add(dutyFromApi);
                }
            }
        }

        private async Task UpdateRemovedDutiesCollection(ObservableCollection<DutyModel> DutiesFromApi)
        {
            List<DutyModel> dutiesToRemove = new List<DutyModel>();

            foreach (DutyModel dutyInCollection in Duties)
            {
                if (!DutiesFromApi.Any(duty => duty.Id == dutyInCollection.Id))
                {
                    dutiesToRemove.Add(dutyInCollection);
                }
            }

            if (dutiesToRemove.Any())
            {
                foreach (DutyModel dutyToRemove in dutiesToRemove)
                {
                    Duties.Remove(dutyToRemove);
                }
            }
        }

        private void UpdateEditedDutiesCollection(ObservableCollection<DutyModel> DutiesFromApi)
        {
            foreach (var dutyInCollection in Duties)
            {
                if (dutyInCollection != null)
                {
                    var dutyFromApi = DutiesFromApi.SingleOrDefault(item => item.Id == dutyInCollection.Id);
                    Console.WriteLine(dutyInCollection.Id);
                    if (dutyFromApi != null)
                    {
                        Console.WriteLine(dutyFromApi.Id);
                        dutyInCollection.DutyDescription = dutyFromApi.DutyDescription;
                        dutyInCollection.Priority = dutyFromApi.Priority;
                        dutyInCollection.Time = dutyFromApi.Time;
                        dutyInCollection.EmployeeId = dutyFromApi.EmployeeId;
                    }
                }
            }
        }

        public override void DecreaseResidualTime(List<int> ListDutyID, double timeToReduce)
        {
            foreach (var dutyID in ListDutyID)
            {
                var dutyToUpdate = Duties.FirstOrDefault(duty => duty.Id == dutyID);
                if (dutyToUpdate != null)
                {
                    if (dutyToUpdate.Time > timeToReduce)
                    {
                        dutyToUpdate.Time -= timeToReduce;
                    }
                    else
                    {
                        timeToReduce -= dutyToUpdate.Time;
                        dutyToUpdate.Time = 0;

                        var employeeID = dutyToUpdate.EmployeeId;
                        var newDutyID = LookForDutyWithTheHighestPriorityForEmployee(employeeID);
                        if (newDutyID != 0)
                        {
                            ListDutyID.Append(newDutyID); // Tutaj znaazłem różnicę między Add a Append, na Add wyskakuje błąd :)  
                        }
                    }
                    OnPropertyChanged(nameof(Duties));
                    RefreshDutiesViews();
                    _apiService.UpdateDuty(dutyToUpdate);
                }
            }
        }
    }
}
