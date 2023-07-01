using System;
using System.Collections.Generic;
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
            Duties.Add(duty);
            await _apiService.PostDuty(duty);
        }

        public override int GenerateNewDutyID()
        {
            return 0; // Pozostawiam inkrementowanie WebApi
        }
    }
}
