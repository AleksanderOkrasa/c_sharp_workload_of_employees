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
        public WorkloadViewModel() : base()
        {
            _apiService = new ApiService("http://127.0.0.1:5052");
            DeleteDutyCommand = new RelayCommand<DutyModel>(DeleteDuty);
            EditDutyCommand = new RelayCommand<DutyModel>(EditDuty);
        }
        public ICommand DeleteDutyCommand { get; private set; }
        public ICommand EditDutyCommand {  get; private set; }

        public override async Task AddDutyToDB(DutyModel duty)
        {
            await _apiService.PostDuty(duty);

            var DutiesFromApi = await _apiService.GetDuties();
            await UpdateNewDutiesCollection(DutiesFromApi);
        }

        public override int GenerateNewDutyID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }

        private async void DeleteDuty(DutyModel duty)
        {
            await _apiService.DeleteDuty(duty.Id);
            var DutiesFromApi = await _apiService.GetDuties();

            await UpdateRemovedDutiesCollection(DutiesFromApi);
        }

        private void EditDuty(DutyModel duty)
        {

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
    }
}
