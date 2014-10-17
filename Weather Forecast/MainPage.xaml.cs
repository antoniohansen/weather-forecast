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
using System.Device.Location;
using System.Threading;

namespace Weather_Forecast
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static Location location;
        public GeoCoordinateWatcher watcher;
        private bool citiesLoaded = false;
        // Constructor
        public MainPage()
        {
            location = new Location(this);
            InitializeComponent();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 70.0f;

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            Thread locService = new Thread(startLocServInBackground);
            locService.Start();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void startLocServInBackground()
        {
            watcher.TryStart(true, TimeSpan.FromSeconds(60));
            location.IsTrackingOn = true;
            location.IsInternetOn = false;

            //watcher_PositionChanged(this, new GeoPositionChangedEventArgs<GeoCoordinate>(watcher.Position));
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //Call function to reload the data from the webservice!
            if (location.IsTrackingOn)
            {
                if(location==null)
                    location = new Location(this);

                location.GetCity(e.Position.Location.Latitude, e.Position.Location.Longitude);
                location.GetForecast(e.Position.Location.Latitude, e.Position.Location.Longitude,true);
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    DataContext = location;
                });
            }
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Your GPS is disabled", "Warning", MessageBoxButton.OK);
                    break;
                case GeoPositionStatus.NoData:
                    MessageBox.Show("No data available", "Warning", MessageBoxButton.OK);
                    break;
            }
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            if (location.IsInternetOn && !citiesLoaded)
                loadCities();
        }

        private void loadCities()
        {
            location.GetForecast(51.500862, -0.126042, false);
            location.GetForecast(40.724885, -74.00528, false);
            location.GetForecast(-23.518663, -46.645203, false);
            location.GetForecast(55.755907, 37.617617, false);
            location.GetForecast(-33.873622, 151.206883, false);
            citiesLoaded = true;
        }

        private void Panorama_Tap(object sender, GestureEventArgs e)
        {
            if (location.IsTrackingOn && location.IsInternetOn)
                watcher_PositionChanged(this, new GeoPositionChangedEventArgs<GeoCoordinate>(watcher.Position));
        }

        private void lstForecastClose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstForecastClose.SelectedItem = null;
        }

        private void internetTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (internetTglBtn.IsChecked == true)
            {
                internetTglBtn.Content = "On";
                location.IsInternetOn = true;
                if(!citiesLoaded && location.IsTrackingOn)
                    loadCities();
            }
            else
            {
                internetTglBtn.Content = "Off";
                location.IsInternetOn = false;
            }
        }

        private void gpsTglBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (gpsTglBtn.IsChecked == true)
            {
                gpsTglBtn.Content = "On";
                location.IsTrackingOn = true;
                if (!citiesLoaded && location.IsInternetOn)
                    loadCities();
            }
            else
            {
                gpsTglBtn.Content = "Off";
                location.IsTrackingOn = false;
            }
        }
    }
}