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
using WpfApp.View;
using WpfApp.ViewModel;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkloadViewModel workloadViewModel;
        private EmployeeManagerViewModel employeeManagerViewModel;
        private EmployeeManagerWindow employeeManagerWindow;

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
    }

}
