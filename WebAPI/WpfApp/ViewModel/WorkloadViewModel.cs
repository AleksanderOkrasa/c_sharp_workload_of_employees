using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Basic;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    internal partial class WorkloadViewModel : ViewModelBase
    {
        public ObservableCollection<TaskModel> Tasks { get; set; } // Przechowywanie zadań
        public List<KeyValuePair<int, string>> PriorityList { get; }
        public Command AddTaskCommand { get; private set; }




        private string newTaskDescription;

        public string NewTaskDescription
        {
            get => newTaskDescription;
            set => Set(ref newTaskDescription, value);
        }

        private KeyValuePair<int, string> _selectedPriority;
        public KeyValuePair<int, string> SelectedPriority
        {
            get => _selectedPriority; 
            set => Set(ref _selectedPriority, value);
        }

        private string numericValue;
        public string NumericValue { get => numericValue; set => Set(ref numericValue, value); }


        public WorkloadViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();

            AddTaskCommand = new Command(AddTask);

            PriorityList = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(1, "Niski"),
            new KeyValuePair<int, string>(2, "Średni"),
            new KeyValuePair<int, string>(3, "Wysoki"),
            new KeyValuePair<int, string>(4, "Bardzo wysoki"),
            new KeyValuePair<int, string>(5, "Krytyczny")
        };
        }

        private void AddTask()
        {
            TaskModel newTask = new TaskModel
            {
                ID = GenerateNewTaskID(),
                TaskDescription = NewTaskDescription,
                Priority = SelectedPriority.Key,
            };

            Tasks.Add(newTask);
            NewTaskDescription = string.Empty;
        }

        private int GenerateNewTaskID()
        {
            if (Tasks.Count > 0)
                return Tasks[Tasks.Count - 1].ID + 1;
            else
                return 1;
        }

 
    }
}
