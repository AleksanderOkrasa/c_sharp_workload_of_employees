using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Basic;

namespace WpfApp.Model
{
    public class DutyModel : ViewModelBase
    {
        private double time;
        public int Id { get; set; }
        public string DutyDescription { get; set; }
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
