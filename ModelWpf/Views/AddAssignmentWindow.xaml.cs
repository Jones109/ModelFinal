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

namespace ModelWpf.Views
{
    /// <summary>
    /// Interaction logic for AddAssignmentWindow.xaml
    /// </summary>
    public partial class AddAssignmentWindow : Window
    {
        public AddAssignmentWindow()
        {
            InitializeComponent();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
