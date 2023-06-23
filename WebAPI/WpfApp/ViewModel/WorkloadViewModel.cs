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
        private string newTaskDescription;
        private KeyValuePair<int, string> _selectedPriority;
        private double numericTimeValue = 1;
        private int selectedEmployeeID;
        private bool automaticTimeLapseIsChecked;
        private ICollectionView _tasksView;

        public WorkloadViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();

            AddTaskCommand = new Command(AddTask);

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

            _tasksView = CollectionViewSource.GetDefaultView(Tasks); // widok task listy
            SortByTimeCommand = new Command(SortByTime);
            SortByEmployeeCommand = new Command(SortByEmployee);
            SortByPriorityCommand = new Command(SortByPriority);
            ClearSortingCommand = new Command(ClearSorting);

        }

        public ObservableCollection<EmployeeModel> Employees { get; set; }
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public List<KeyValuePair<int, string>> PriorityList { get; }
        public Command AddTaskCommand { get; private set; }
        public Command LetAnHourPassCommand { get; private set; }
        public Command AutomaticTimeLapseCommand { get; private set; }
        public Command SortByTimeCommand { get; private set; }
        public Command SortByEmployeeCommand { get; private set; }
        public Command SortByPriorityCommand { get; private set; }
        public Command ClearSortingCommand { get; private set; }  



        public string NewTaskDescription { get => newTaskDescription; set => Set(ref newTaskDescription, value); }

        public KeyValuePair<int, string> SelectedPriority { get => _selectedPriority; set => Set(ref _selectedPriority, value); }

        public double NumericTimeValue { get => numericTimeValue; set => Set(ref numericTimeValue, value); }

        public int SelectedEmployeeID { get => selectedEmployeeID; set => Set(ref selectedEmployeeID, value); }

        public bool AutomaticTimeLapseIsChecked { get => automaticTimeLapseIsChecked; set => Set(ref automaticTimeLapseIsChecked, value); }


        private void AddTask()
        {
            TaskModel newTask = new TaskModel
            {
                ID = GenerateNewTaskID(),
                TaskDescription = NewTaskDescription,
                Priority = SelectedPriority.Key,
                Time = NumericTimeValue,
                EmployeeId = SelectedEmployeeID,
            };

            Tasks.Add(newTask);

            NewTaskDescription = string.Empty;
            NumericTimeValue = 1;
            SelectedPriority = PriorityList.FirstOrDefault();
            _tasksView.Refresh();

        }

        private int GenerateNewTaskID()
        {
            if (Tasks.Count > 0)
                return Tasks[Tasks.Count - 1].ID + 1;
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
            List<int> ListTaskIdToTimeChange = new List<int>();

            foreach (var employee in Employees)
            {
                int employeeID = employee.ID;
                var taskID = LookForTaskWithTheHighestPriorityForEmployee(employeeID);
                if (taskID != 0)
                {
                    ListTaskIdToTimeChange.Add(taskID);
                }
            }
            DecreaseResidualTime(ListTaskIdToTimeChange, timeToReduce);

        }

        private void LetAnHourPass()
        {
            LetAnTimePass(1);
        }

        private int LookForTaskWithTheHighestPriorityForEmployee(int employeeID)
        {
            for (var i = 5; i >= 1; i--)
            {
                var taskID = SearchPriorityInTaskForEmployee(employeeID, i);
                if (taskID != 0) { return taskID; }
            }
            return 0;
        }

        private int SearchPriorityInTaskForEmployee(int employeeID, int priority)
        {
            foreach (var task in Tasks)
            {
                if (task.EmployeeId == employeeID && task.Priority == priority && task.Time > 0)
                {
                    return task.ID;
                }
            }
            return 0;
        }


        private void DecreaseResidualTime(List<int> ListTaskID, double timeToReduce)
        {
            foreach (var taskID in ListTaskID)
            {
                var taskToUpdate = Tasks.FirstOrDefault(task => task.ID == taskID);
                if (taskToUpdate != null)
                {
                    if (taskToUpdate.Time > timeToReduce)
                    {
                        taskToUpdate.Time -= timeToReduce;
                    }
                    else
                    {
                        timeToReduce -= taskToUpdate.Time;
                        taskToUpdate.Time = 0;

                        var employeeID = taskToUpdate.EmployeeId;
                        var newTaskID = LookForTaskWithTheHighestPriorityForEmployee(employeeID);
                        if (newTaskID != 0)
                        {
                            ListTaskID.Append(newTaskID); // Tutaj znalazłem różnicę między Add a Append, na Add wyskakuje błąd :)  
                        }
                    }
                    OnPropertyChanged(nameof(Tasks));
                    _tasksView.Refresh();
                }
            }
        }
        private void SortByTime()
        {
            _tasksView.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Descending));
            _tasksView.Refresh();
        }
        private void SortByEmployee()
        {
            _tasksView.SortDescriptions.Add(new SortDescription("EmployeeID", ListSortDirection.Ascending));
            _tasksView.Refresh();

        }
        private void SortByPriority()
        {
            _tasksView.SortDescriptions.Add(new SortDescription("Priority", ListSortDirection.Descending));
            _tasksView.Refresh();
        }
        private void ClearSorting()
        {
            _tasksView.SortDescriptions.Clear();
            _tasksView.Refresh();
        }

    }
}
