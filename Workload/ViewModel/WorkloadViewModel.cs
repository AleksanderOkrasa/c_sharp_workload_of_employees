using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workload.Models;
using Workload.Services;


namespace Workload.ViewModel
{
    internal class WorkloadViewModel : WpfApp.ViewModel.WorkloadViewModel
    {
        private ApiService _apiService;
        public WorkloadViewModel() : base()
        {
            _apiService = new ApiService("http://127.0.0.1:5052");
        }


        public override async Task AddDutyToDB(DutyModel duty)
        {
            await _apiService.PostDuty(duty);
            var DutiesFromApi = await _apiService.GetDuties();
            UpdateDutiesCollection(DutiesFromApi);
        }
        
        private void UpdateDutiesCollection(ObservableCollection<DutyModel> DutiesFromApi)
        {
            foreach (DutyModel dutyFromApi in DutiesFromApi)
            {
                if (!Duties.Any(duty => duty.Id == dutyFromApi.Id))
                {
                    Duties.Add(dutyFromApi);
                }
            }
        }

        public override int GenerateNewDutyID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }
    }
}
