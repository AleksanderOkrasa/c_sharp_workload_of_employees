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

        private string editedDutyDescription;
        private int editedPriority;
        private KeyValuePair<int, string> _editedPriority;
        private double editedTimeValue;
        private int editedEmployeeID;


        public EditDutyViewModel(DutyModel selectedDuty)
        {
            editedDutyDescription = selectedDuty.DutyDescription;
            editedPriority = selectedDuty.Priority;
            editedTimeValue = selectedDuty.Time;
            editedEmployeeID = selectedDuty.EmployeeId;
        }

        public string EditedDutyDescription { get => editedDutyDescription; set => Set(ref editedDutyDescription, value); }
        public int EditedPriority { get => editedPriority; set => Set(ref editedPriority, value); }
        public KeyValuePair<int, string> EditedPriorityDict { get => _editedPriority; set => Set(ref _editedPriority, value); }
        public double EditedTimeValue { get => editedTimeValue; set => Set(ref editedTimeValue, value); }
        public int EditedEmployeeID { get => editedEmployeeID; set => Set(ref editedEmployeeID, value); }

        public DutyModel SelectedDuty { get => _selectedDuty; set => Set(ref _selectedDuty, value); }
        public Command EditDutyCommand { get; set; }
    }

}
