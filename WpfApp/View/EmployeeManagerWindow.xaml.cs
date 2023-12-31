﻿using System;
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
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy EmployeeManagerWindow.xaml
    /// </summary>
    public partial class EmployeeManagerWindow : Window
    {
        private EmployeeManagerViewModel employeeManagerViewModel;

        public EmployeeManagerWindow(EmployeeManagerViewModel viewModel)
        {
            InitializeComponent();
            employeeManagerViewModel = viewModel;
            DataContext = employeeManagerViewModel;
        }
    }
}
