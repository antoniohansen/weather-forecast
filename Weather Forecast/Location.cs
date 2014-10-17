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
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using System.Threading;
using System.Globalization;

namespace Weather_Forecast
{
    public class Location : INotifyPropertyChanged
    {
        #region Variáveis
        private bool trackingOn = false;
        private bool internetOn = false;
        private MainPage mainPage;
        private City city;
        private int count = 0;
        private IEnumerable<CityForecast> cityCloseColl;
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<int, string> cit = new Dictionary<int, string>{
            {1,"London"},{2,"New York"},{3,"São Paulo"},{4,"Moscow"},{5,"Sydney"}
        };

        public Location(MainPage mainPage)
        {
            this.mainPage = mainPage;
            city = new City();
            CityCloseColl = new List<CityForecast>();
        }

        #endregion

        #region Properties

        public City City
        {
            get { return city; }
            set { if (!value.Equals(city)) { city = value; NotifyPropertyChanged("City"); } }
        }

        public bool IsTrackingOn
        {
            get { return trackingOn; }
            set{ if(!value.Equals(trackingOn)){trackingOn=value; NotifyPropertyChanged("IsTrackingOn");}}
        }

        public IEnumerable<CityForecast> CityCloseColl
        {
            get { return cityCloseColl; }
            set { if (!value.Equals(cityCloseColl)) { cityCloseColl = value; NotifyPropertyChanged("CityCloseColl"); } }
        }

        public bool IsInternetOn
        {
            get { return internetOn; }
            set { if (!value.Equals(internetOn)) { internetOn = value; NotifyPropertyChanged("IsInternetOn"); } }
        }

        #endregion

        #region Handlers
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private void city_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && e.Result!=null)
            {
                XElement xmlCity = XElement.Parse(e.Result);
                IEnumerable<XElement> responses = xmlCity.Descendants();

                foreach(XElement response in responses)
                {
                    if(response.Name.LocalName.Equals("Locality"))
                        City.CityName = response.Value;
                    if(response.Name.LocalName.Equals("FormattedAddress"))
                        City.FormattedAdd = response.Value;
                }
            }
        }

        private void weatherClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                XElement xmlWeather = XElement.Parse(e.Result);
                var dados = from forecast in xmlWeather.Descendants("weather")
                            select new CityForecast
                            {
                                City = this.City,
                                ImageSource= forecast.Element("weatherIconUrl").Value,
                                TempMax=forecast.Element("tempMaxC").Value + " ºC",
                                TempMin=forecast.Element("tempMinC").Value + " ºC",
                                WindDirection = forecast.Element("winddirection").Value,
                                WindSpeed = forecast.Element("windspeedKmph").Value,
                                Forecast = forecast.Element("weatherDesc").Value
                                
                            };
                mainPage.lstForecast.ItemsSource = dados;
            }
        }

        private void weatherClientSecond_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                XElement xmlWeather = XElement.Parse(e.Result);
                var dados = from forecast in xmlWeather.Descendants("weather")
                            select new CityForecast
                            {
                                City = new City() { CityName = cit[count] },
                                ImageSource = forecast.Element("weatherIconUrl").Value,
                                TempMax = forecast.Element("tempMaxC").Value + " ºC",
                                TempMin = forecast.Element("tempMinC").Value + " ºC",
                                Forecast = forecast.Element("weatherDesc").Value

                            };
                count++;
                var aux = CityCloseColl.ToList();
                aux.Add(dados.First());
                CityCloseColl = aux;
                mainPage.lstForecastClose.ItemsSource = (IEnumerable<CityForecast>)cityCloseColl;
            }
        }
        #endregion

        public void GetCity(double latitude, double longitude)
        {
            if (IsInternetOn)
            {
                WebClient cityClient = new WebClient();

                //Using BingMaps!
                //http://dev.virtualearth.net/REST/v1/Locations/47.64054,-122.12934?o=xml&key=Amabb9o-zzECLDQmDGZgoAlv1P1bDhwF_-8kqHHoSPPqo8-4N7g1ptppF5ENNY4Q
                UriBuilder fullUri = new UriBuilder("http://dev.virtualearth.net/REST/v1/Locations/" + latitude + "," + longitude);
                fullUri.Query = "o=xml&key=Amabb9o-zzECLDQmDGZgoAlv1P1bDhwF_-8kqHHoSPPqo8-4N7g1ptppF5ENNY4Q";

                cityClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(city_DownloadStringCompleted);
                cityClient.DownloadStringAsync(fullUri.Uri);
            }
        }

        public void GetForecast(double latitude, double longitude, bool gps)
        {
            if (IsInternetOn)
            {
                //Using FreeWorldWeather
                //Geting the weather from places
                //http://free.worldweatheronline.com/feed/weather.ashx?q=-30.13,-51.10&format=xml&num_of_days=5&key=f42f9b2e6f185546110711
                WebClient weatherClient = new WebClient();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                UriBuilder fullUri = new UriBuilder("http://free.worldweatheronline.com/feed/weather.ashx");
                fullUri.Query = "q=" + latitude.ToString() + "," + longitude.ToString() + "&format=xml&num_of_days=" + (gps ? 5 : 1) + "&key=f42f9b2e6f185546110711";

                if (gps)
                {
                    weatherClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(weatherClient_DownloadStringCompleted);
                }
                else
                {
                    weatherClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(weatherClientSecond_DownloadStringCompleted);
                }
                weatherClient.DownloadStringAsync(fullUri.Uri);
            }
        }
    }


    


    #region CityUpdateState
    public class CityUpdateState
    {
        public HttpWebRequest AsyncRequest { get; set; }
        public HttpWebResponse AsyncResponse { get; set; }
    }
    #endregion
}
