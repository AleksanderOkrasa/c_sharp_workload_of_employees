using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Workload.Models;
using WpfApp.Basic;

namespace Workload.ViewModel
{
    internal class EditDutyViewModel : WorkloadViewModel
    {
        private DutyModel selectedDuty;
        private WorkloadViewModel workloadViewModel;

        private int editedDutyId;
        private string editedDutyDescription;
        private KeyValuePair<int, string> editedPriority;
        private double editedTimeValue;
        private int editedEmployeeId;


        public EditDutyViewModel(WorkloadViewModel workloadViewModel, DutyModel selectedDuty)
        {
            this.workloadViewModel = workloadViewModel;
            this.selectedDuty = selectedDuty;
            Employees = workloadViewModel.Employees;
            EditDutyCommand = new Command(EditDuty);

            editedDutyId = selectedDuty.Id;
            editedDutyDescription = selectedDuty.DutyDescription;
            editedPriority = PriorityList.FirstOrDefault(pair => pair.Key == selectedDuty.Priority);
            editedTimeValue = selectedDuty.Time;
            editedEmployeeId = selectedDuty.EmployeeId;
        }

        public int EditedDutyId { get => editedDutyId; set => Set(ref editedDutyId, value); }
        public string EditedDutyDescription { get => editedDutyDescription; set => Set(ref editedDutyDescription, value); }
        public KeyValuePair<int, string> EditedPriority { get => editedPriority; set => Set(ref editedPriority, value); }
        public double EditedTimeValue { get => editedTimeValue; set => Set(ref editedTimeValue, value); }
        public int EditedEmployeeId { get => editedEmployeeId; set => Set(ref editedEmployeeId, value); }


        public DutyModel SelectedDuty { get => selectedDuty; set => Set(ref selectedDuty, value); }
        public Command EditDutyCommand { get; set; }
        public ObservableCollection<EmployeeModel> Employees { get; set; }

        
        private async void EditDuty()
        {
            DutyModel editedDuty = new DutyModel()
            {
                Id = EditedDutyId,
                DutyDescription = EditedDutyDescription,
                Priority = EditedPriority.Key,
                Time = EditedTimeValue,
                EmployeeId = EditedEmployeeId,
            };
            await workloadViewModel.UpdateDuty(editedDuty);
        }
    }

}
