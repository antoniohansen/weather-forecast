using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Weather_Forecast
{
    public class City : INotifyPropertyChanged
    {
        private string cityName, formattedAdd;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CityName
        {
            get { return this.cityName; }
            set { if (!Equals(value, this.cityName)) { this.cityName = value; NotifyPropertyChanged("CityName"); } }
        }

        public string FormattedAdd
        {
            get { return this.formattedAdd; }
            set { if (!Equals(value, this.formattedAdd)) { this.formattedAdd = value; NotifyPropertyChanged("FormattedAdd"); } }
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }
}
