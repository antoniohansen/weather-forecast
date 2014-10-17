using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Weather_Forecast
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void internetTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (internetTglBtn.IsChecked == true)
            {
                internetTglBtn.Content = "Off";
            }
            else
            {
                internetTglBtn.Content = "On";
            }
        }

        private void gpsTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (gpsTglBtn.IsChecked == true)
            {
                gpsTglBtn.Content = "Off";
            }
            else
            {
                gpsTglBtn.Content = "On";
            }
        }
    }
}