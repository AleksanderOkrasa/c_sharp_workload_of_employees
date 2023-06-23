using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Basic;

namespace WpfApp.Model
{
    public class TaskModel : ViewModelBase
    {
        private double time;
        public int ID { get; set; }
        public string TaskDescription { get; set; }
        public int EmployeeId { get; set; }
        public int Priority { get; set; }
        public double Time { get { return time; }
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

    }
}
