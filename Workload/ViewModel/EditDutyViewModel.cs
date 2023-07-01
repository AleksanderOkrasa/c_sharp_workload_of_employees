using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workload.Models;
using WpfApp.Basic;

namespace Workload.ViewModel
{
    internal class EditDutyViewModel : WorkloadViewModel
    {
        private DutyModel _selectedDuty;

        public EditDutyViewModel(DutyModel selectedDuty)
        {
            this._selectedDuty = selectedDuty;
        }

        public DutyModel SelectedDuty { get => _selectedDuty; set => Set(ref _selectedDuty, value); }
    }

}
