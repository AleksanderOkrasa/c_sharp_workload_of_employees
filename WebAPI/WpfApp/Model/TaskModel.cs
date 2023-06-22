using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Model
{
    internal class TaskModel
    {
        public int ID { get; set; }
        public string TaskDescription { get; set; }
        public int EmployeeId { get; set; }
        public double Time { get; set; }
        public int Priority { get; set; }
    }
}
