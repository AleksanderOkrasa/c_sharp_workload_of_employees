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

        private string newTaskDescription;
        public string NewTaskDescription
        {
            get { return newTaskDescription; }
            set
            {
                if (newTaskDescription != value)
                {
                    newTaskDescription = value;
                    OnPropertyChanged("NewTaskDescription");
                }
            }
        }

        public Command AddTaskCommand { get; private set; }

        public WorkloadViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();

            AddTaskCommand = new Command(AddTask);
        }

        private void AddTask()
        {
            TaskModel newTask = new TaskModel
            {
                ID = GenerateNewTaskID(),
                TaskDescription = NewTaskDescription,
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
