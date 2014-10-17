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
    public class CityForecast : INotifyPropertyChanged
    {
        private string imageSource, tempMax, tempMin, windDirection, windSpeed,forecast;
        private City city;
        public event PropertyChangedEventHandler PropertyChanged;
        public City City { get { return this.city; } set { if (!Equals(value, this.city)) { this.city = value; NotifyPropertyChanged("City"); } } }
        public string ImageSource { get { return this.imageSource; } set { if (!Equals(value, this.imageSource)) { this.imageSource = value; NotifyPropertyChanged("ImageSource"); } } }
        public string TempMax { get { return this.tempMax; } set { if (!Equals(value, this.tempMax)) { this.tempMax = value; NotifyPropertyChanged("TempMax"); } } }
        public string TempMin { get { return this.tempMin; } set { if (!Equals(value, this.tempMin)) { this.tempMin = value; NotifyPropertyChanged("TempMin"); } } }
        public string Forecast { get { return this.forecast; } set { if (!Equals(value, this.forecast)) { this.forecast = value; NotifyPropertyChanged("Forecast"); } } }
        public string WindDirection
        {
            get { return this.windDirection; }
            set
            {
                switch (value)
                {
                    case "N":
                        if (!Equals(value, windDirection))
                        {
                            this.windDirection = value;
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "S":
                        if (!Equals(value, windDirection))
                        {
                            this.windDirection = value;
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "E":
                        if (!Equals("L", windDirection))
                        {
                            this.windDirection = "L";
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "W":
                        if (!Equals("O", windDirection))
                        {
                            this.windDirection = "O";
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "NE":
                        if (!Equals(value, windDirection))
                        {
                            this.windDirection = value;
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "SE":
                        if (!Equals(value, windDirection))
                        {
                            this.windDirection = value;
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "NW":
                        if (!Equals("NO", windDirection))
                        {
                            this.windDirection = "NO";
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    case "SW":
                        if (!Equals("SO", windDirection))
                        {
                            this.windDirection = "SO";
                            NotifyPropertyChanged("WindDirection");
                        }
                        break;
                    default: break;
                }
            }
        }
        public string WindSpeed { get { return this.windSpeed; } set { if (!Equals(value, this.windSpeed)) { this.windSpeed = value; NotifyPropertyChanged("WindSpeed"); } } }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }
}
