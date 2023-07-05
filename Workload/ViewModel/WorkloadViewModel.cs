using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Workload.View;
using Workload.Models;
using Workload.Services;
using Workload.Basic;
using System.ComponentModel;
using System.Windows.Data;

namespace Workload.ViewModel
{
    internal class WorkloadViewModel : ViewModelBase
    {
        private ApiService _apiService;
        private DutyModel editedDuty;
        private ICollectionView _dutiesView;
        private bool automaticTimeLapseIsChecked;
        private KeyValuePair<int, string> _selectedPriority;
        private int filterEmployeeID;
        private string newDutyDescription;
        private double numericTimeValue;
        private int selectedEmployeeID;

        public WorkloadViewModel()
        {
            _apiService = new ApiService("http://127.0.0.1:5052");
            DeleteDutyCommand = new RelayCommand<DutyModel>(DeleteDuty);
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
            SortByDutyIdCommand = new Command(SortByDutyId);
            ClearSortingAndFiltersCommand = new Command(ClearSortingAndFilters);
            FilterByEmployeeCommand = new Command(FilterByEmployee);
            LoadDataFromApi();
        }

        private void ClearSortingAndFilters()
        {
            _dutiesView.SortDescriptions.Clear();
            _dutiesView.Filter = null;
            FilterEmployeeID = 0;
            SortByDutyId();
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
        private void SortByDutyId()
        {
            _dutiesView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            _dutiesView.Refresh();
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


        public ICommand DeleteDutyCommand { get; private set; }
        public DutyModel EditedDuty { get => editedDuty; set => Set(ref editedDuty, value); }


        public async Task AddDutyToDB(DutyModel duty)
        {
            await _apiService.PostDuty(duty);

            var dutiesFromApi = await _apiService.GetDuties();
            await UpdateNewDutiesCollection(dutiesFromApi);
        }

        public int GenerateNewDutyID()
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
            foreach (DutyModel duty in Duties)
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

        public void DecreaseResidualTime(List<int> ListDutyID, double timeToReduce)
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

        public int LookForDutyWithTheHighestPriorityForEmployee(int employeeID)
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

        public void RefreshDutiesViews()
        {
            _dutiesView.Refresh();
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
        public Command SortByDutyIdCommand { get; set; }
        public Command ClearSortingAndFiltersCommand { get; private set; }
        public Command FilterByEmployeeCommand { get; private set; }
        public bool AutomaticTimeLapseIsChecked { get => automaticTimeLapseIsChecked; set => Set(ref automaticTimeLapseIsChecked, value); }
        public KeyValuePair<int, string> SelectedPriority { get => _selectedPriority; set => Set(ref _selectedPriority, value); }
        public int FilterEmployeeID { get => filterEmployeeID; set => Set(ref filterEmployeeID, value); }
        public string NewDutyDescription { get => newDutyDescription; set => Set(ref newDutyDescription, value); }
        public double NumericTimeValue { get => numericTimeValue; set => Set(ref numericTimeValue, value); }
        public int SelectedEmployeeID { get => selectedEmployeeID; set => Set(ref selectedEmployeeID, value); }
    }
}

