using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp.Basic;
using WpfApp.Model;
using WpfApp.View;

namespace WpfApp.ViewModel
{
    public partial class WorkloadViewModel : ViewModelBase

    {
        private string newDutyDescription;
        private KeyValuePair<int, string> _selectedPriority;
        private double numericTimeValue = 1;
        private int selectedEmployeeID;
        private bool automaticTimeLapseIsChecked;
        private ICollectionView _dutiesView;
        private int filterEmployeeID;


        public WorkloadViewModel()
        {
            Duties = new ObservableCollection<DutyModel>();

            AddDutyCommand = new Command(AddDuty);

            LetAnHourPassCommand = new Command(LetAnHourPass);

            AutomaticTimeLapseCommand = new Command(AutomaticTimeLapse);

            PriorityList = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, "Niski"),
                new KeyValuePair<int, string>(2, "Średni"),
                new KeyValuePair<int, string>(3, "Wysoki"),
                new KeyValuePair<int, string>(4, "Bardzo wysoki"),
                new KeyValuePair<int, string>(5, "Krytyczny")
            };
            SelectedPriority = PriorityList.FirstOrDefault();

            Employees = new ObservableCollection<EmployeeModel>();

            _dutiesView = CollectionViewSource.GetDefaultView(Duties); 
            SortByTimeCommand = new Command(SortByTime);
            SortByEmployeeCommand = new Command(SortByEmployee);
            SortByPriorityCommand = new Command(SortByPriority);
            ClearSortingAndFiltersCommand = new Command(ClearSortingAndFilters);
            FilterByEmployeeCommand = new Command(FilterByEmployee);

        }

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<DutyModel> Duties { get; set; }
        public List<KeyValuePair<int, string>> PriorityList { get; }
        public Command AddDutyCommand { get; private set; }
        public Command LetAnHourPassCommand { get; private set; }
        public Command AutomaticTimeLapseCommand { get; private set; }
        public Command SortByTimeCommand { get; private set; }
        public Command SortByEmployeeCommand { get; private set; }
        public Command SortByPriorityCommand { get; private set; }
        public Command ClearSortingAndFiltersCommand { get; private set; }
        public Command FilterByEmployeeCommand { get; private set; }   



        public string NewDutyDescription { get => newDutyDescription; set => Set(ref newDutyDescription, value); }

        public KeyValuePair<int, string> SelectedPriority { get => _selectedPriority; set => Set(ref _selectedPriority, value); }

        public double NumericTimeValue { get => numericTimeValue; set => Set(ref numericTimeValue, value); }

        public int SelectedEmployeeID { get => selectedEmployeeID; set => Set(ref selectedEmployeeID, value); }

        public bool AutomaticTimeLapseIsChecked { get => automaticTimeLapseIsChecked; set => Set(ref automaticTimeLapseIsChecked, value); }
        public int FilterEmployeeID { get => filterEmployeeID; set => Set(ref filterEmployeeID, value); }



        private async void AddDuty()
        {
            DutyModel newDuty = new DutyModel
            {
                Id = GenerateNewDutyID(),
                DutyDescription = NewDutyDescription,
                Priority = SelectedPriority.Key,
                Time = NumericTimeValue,
                EmployeeId = SelectedEmployeeID,
            };

            await AddDutyToDB(newDuty);

            NewDutyDescription = string.Empty;
            NumericTimeValue = 1;
            SelectedPriority = PriorityList.FirstOrDefault();
            _dutiesView.Refresh();

        }
         
        private async Task AddDutyToDB(DutyModel duty)
        {
            Duties.Add(duty);
        }

        private int GenerateNewDutyID()
        {
            if (Duties.Count > 0)
                return Duties[Duties.Count - 1].Id + 1;
            else
                return 1;
        }


        private async void AutomaticTimeLapse()
        {
            while (AutomaticTimeLapseIsChecked)
            {
                LetAnTimePass(0.02);
                await Task.Delay(100);
            }

        }


        private void LetAnTimePass(double timeToReduce)
        {
            List<int> ListDutyIdToTimeChange = new List<int>();

            foreach (var employee in Employees)
            {
                int employeeID = employee.Id;
                var dutyID = LookForDutyWithTheHighestPriorityForEmployee(employeeID);
                if (dutyID != 0)
                {
                    ListDutyIdToTimeChange.Add(dutyID);
                }
            }
            DecreaseResidualTime(ListDutyIdToTimeChange, timeToReduce);

        }

        private void LetAnHourPass()
        {
            LetAnTimePass(1);
        }

        private int LookForDutyWithTheHighestPriorityForEmployee(int employeeID)
        {
            for (var i = 5; i >= 1; i--)
            {
                var dutyID = SearchPriorityInDutyForEmployee(employeeID, i);
                if (dutyID != 0) { return dutyID; }
            }
            return 0;
        }

        private int SearchPriorityInDutyForEmployee(int employeeID, int priority)
        {
            foreach (var duty in Duties)
            {
                if (duty.EmployeeId == employeeID && duty.Priority == priority && duty.Time > 0)
                {
                    return duty.Id;
                }
            }
            return 0;
        }


        private void DecreaseResidualTime(List<int> ListDutyID, double timeToReduce)
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
                            ListDutyID.Append(newDutyID); // Tutaj znalazłem różnicę między Add a Append, na Add wyskakuje błąd :)  
                        }
                    }
                    OnPropertyChanged(nameof(Duties));
                    _dutiesView.Refresh();
                }
            }
        }
        private void SortByTime()
        {
            _dutiesView.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Descending));
            _dutiesView.Refresh();
        }
        private void SortByEmployee()
        {
            _dutiesView.SortDescriptions.Add(new SortDescription("EmployeeID", ListSortDirection.Ascending));
            _dutiesView.Refresh();

        }
        private void SortByPriority()
        {
            _dutiesView.SortDescriptions.Add(new SortDescription("Priority", ListSortDirection.Descending));
            _dutiesView.Refresh();
        }
        private void ClearSortingAndFilters()
        {
            _dutiesView.SortDescriptions.Clear();
            _dutiesView.Filter = null;
            _dutiesView.Refresh();
        }

        private void FilterByEmployee()
        {
            _dutiesView.Filter = duty =>
            {
                if (duty is DutyModel dutyModel)
                {
                    return dutyModel.EmployeeId == FilterEmployeeID;
                }
                return false;
            };
            _dutiesView.Refresh();
        }
    }
}
