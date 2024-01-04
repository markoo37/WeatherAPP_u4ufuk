using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace Weather
{
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "b5b4fb118caa4bf4b06223315240101";
        private string cityName;

        
        


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(String properyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private async void btnGetWeather_Click(object sender, RoutedEventArgs e)
        {
            cityName = cityTxtName.Text.Trim();
            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a city name");
                return;
            }

            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}";
            string apiUrlAstronomy = $"http://api.weatherapi.com/v1/astronomy.json?key={apiKey}&q={cityName}";

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiUrl);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                             WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                            DisplayWeatherData(weatherData);
                        }
                    }
                }
            }
            catch (WebException ex)
            {

                MessageBox.Show("An error occured while fetching weather data: " + ex.Message);
            }

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiUrlAstronomy);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            AstronomyData astronomyData = JsonConvert.DeserializeObject<AstronomyData>(jsonResponse);
                            DisplayAstronomyData(astronomyData);
                        }
                    }
                }
            }
            catch (WebException ex)
            {

                MessageBox.Show("An error occured while fetching weather data: " + ex.Message);
            }
        }


        private void DisplayAstronomyData(AstronomyData astronomyData)
        {
            //sunrise/sunset

            lblSunrise.Text = astronomyData.Astronomy.Astro.Sunrise;
            lblSunset.Text = astronomyData.Astronomy.Astro.Sunset;
        }
        private void DisplayWeatherData(WeatherData weatherData)
        {
            //leftSide

            borderLabel.Visibility = Visibility.Visible;
            detailSeparator.Visibility = Visibility.Visible;
            lblCityName.Content = weatherData.Location.Name;
            lblTemperature.Content = (Math.Round(weatherData.Current.TempC)).ToString() + " °C";
            lblCondition.Content = weatherData.Current.Condition.Text;
            lblClock.Content = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            

            //windspeed

            textWindSpeed.Text = weatherData.Current.WindSpeed.ToString();

            //humidity

            humidityText.Text = weatherData.Current.Humidity.ToString();
            int humidity = int.Parse(weatherData.Current.Humidity.ToString());
            humiditySlider.Value = humidity/10;

            //uv

            uvSlider.Value = weatherData.Current.UV;
            uvText.Text = $"Avarage UV index is {weatherData.Current.UV}";

            //image

            BitmapImage weatherIcon = new BitmapImage();
            weatherIcon.BeginInit();
            weatherIcon.UriSource = new Uri("http:" + weatherData.Current.Condition.Icon);
            weatherIcon.EndInit();
            imageWeatherIcon.Source = weatherIcon;
        }
    }
}