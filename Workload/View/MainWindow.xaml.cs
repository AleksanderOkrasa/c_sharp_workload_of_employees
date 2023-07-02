using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Workload.ViewModel;
using Workload.View;
using Workload.Models;

namespace Workload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkloadViewModel workloadViewModel;

        private EmployeeManagerViewModel employeeManagerViewModel;
        private EmployeeManagerWindow employeeManagerWindow;

        private EditDutyViewModel editDutyViewModel;
        private EditDutyWindow editDutyWindow;

        public MainWindow()
        {
            InitializeComponent();
            workloadViewModel = new WorkloadViewModel();
            employeeManagerViewModel = new EmployeeManagerViewModel(workloadViewModel);
            DataContext = workloadViewModel;
        }

        private void OpenEmployeeManagerWindow(object sender, RoutedEventArgs e)
        {
            employeeManagerWindow = new EmployeeManagerWindow(employeeManagerViewModel);
            employeeManagerWindow.Show();
        }

        private void OpenEditDutyWindow(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var duty = (DutyModel)button.CommandParameter;

            editDutyViewModel = new EditDutyViewModel(workloadViewModel, duty);
            editDutyWindow = new EditDutyWindow(editDutyViewModel);

            editDutyWindow.Show();
        }


    }
}
