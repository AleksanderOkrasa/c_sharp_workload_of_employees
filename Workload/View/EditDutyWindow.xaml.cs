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
using System.Windows.Shapes;
using Workload.Models;
using Workload.ViewModel;
using WpfApp.ViewModel;

namespace Workload.View
{
    /// <summary>
    /// Logika interakcji dla klasy EditDutyWindow.xaml
    /// </summary>
    public partial class EditDutyWindow : Window
    {
        private EditDutyViewModel editDutyViewModel;
        internal EditDutyWindow(EditDutyViewModel viewModel)
        {
            InitializeComponent();
            editDutyViewModel = viewModel;
            DataContext = editDutyViewModel;
            LoadStyles();
        }

        private void LoadStyles()
        {
            ResourceDictionary styles = new ResourceDictionary();
            styles.Source = new Uri("View/Styles.xaml", UriKind.RelativeOrAbsolute);

            this.Resources.MergedDictionaries.Add(styles);
        }
        private void CloseEditDutyWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}