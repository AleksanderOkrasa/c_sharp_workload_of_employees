using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workload.Models
{
    public class DutyModel
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
            }
        }

    }
}
