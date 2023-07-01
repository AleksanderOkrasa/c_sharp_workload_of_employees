using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Workload.ViewModel
{
    internal class EmployeeManagerViewModel : WpfApp.ViewModel.EmployeeManagerViewModel
    {
        public EmployeeManagerViewModel(WpfApp.ViewModel.WorkloadViewModel workloadViewModel) : base(workloadViewModel)
        {
        }
    }
}
