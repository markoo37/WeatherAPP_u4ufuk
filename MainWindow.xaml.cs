using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
            cityTxtName.Text = "";
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


            try
            {
                DateTime currentDay = DateTime.Now;

                //HttpWebRequest request = WebRequest.CreateHttp(apiUrlFuture);
                //request.Method = "GET";

                //using (WebResponse response = await request.GetResponseAsync())
                //{
                //    using (Stream stream = response.GetResponseStream())
                //    {
                //        using (StreamReader reader = new StreamReader(stream))
                //        {
                //            string jsonResponse = reader.ReadToEnd();
                //            FutureData futureData = JsonConvert.DeserializeObject<FutureData>(jsonResponse);
                //            DisplayFutureData(futureData, 1);
                //        }
                //    }
                //}


                for (int i = 1; i < 8; i++)
                {                    
                    //{ currentDay.AddDays(i).ToString("yyyy-MM-dd")}
                    string apiUrlFuture = $"http://api.weatherapi.com/v1/future.json?key={apiKey}&q={cityName}&dt={currentDay.AddDays(13 + i).ToString("yyyy-MM-dd")}";
                    HttpWebRequest request = WebRequest.CreateHttp(apiUrlFuture);
                    request.Method = "GET";

                    using (WebResponse response = await request.GetResponseAsync())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string jsonResponse = reader.ReadToEnd();
                                FutureData futureData = JsonConvert.DeserializeObject<FutureData>(jsonResponse);
                                DisplayFutureData(futureData, i, currentDay.AddDays(13 + i).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

            }
            catch (WebException ex)
            {

                MessageBox.Show("An error occured while fetching forecast weather data: " + ex.Message);
            }
            
        }


        private void DisplayAstronomyData(AstronomyData astronomyData)
        {
            //sunrise/sunset

            lblSunrise.Text = astronomyData.Astronomy.Astro.Sunrise;
            lblSunset.Text = astronomyData.Astronomy.Astro.Sunset;
        }

        private void DisplayFutureData(FutureData futureData, int daysFromNow, string date)
        {
            ForecastDay forecastDay = futureData.Forecast.ForecastDay.First();
            BitmapImage weatherIcon = new BitmapImage();
            switch (daysFromNow)
            {
                case 1:
                    day1.Visibility = Visibility.Visible;
                    day1.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day1.MinNum = forecastDay.Day.MinTemp.ToString();

                    day1.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day1.Source = weatherIcon;


                    break;
                case 2:
                    day2.Visibility = Visibility.Visible;

                    day2.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day2.MinNum = forecastDay.Day.MinTemp.ToString();

                    day2.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day2.Source = weatherIcon;

                    break;
                case 3:
                    day3.Visibility = Visibility.Visible;

                    day3.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day3.MinNum = forecastDay.Day.MinTemp.ToString();

                    day3.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day3.Source = weatherIcon;

                    break;
                case 4:
                    day4.Visibility = Visibility.Visible;

                    day4.Day = date;

                    day4.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day4.MinNum = forecastDay.Day.MinTemp.ToString();

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day4.Source = weatherIcon;

                    break;
                case 5:
                    day5.Visibility = Visibility.Visible;

                    day5.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day5.MinNum = forecastDay.Day.MinTemp.ToString();

                    day5.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day5.Source = weatherIcon;

                    break;
                case 6:
                    day6.Visibility = Visibility.Visible;

                    day6.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day6.MinNum = forecastDay.Day.MinTemp.ToString();

                    day6.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day6.Source = weatherIcon;

                    break;
                case 7:
                    day7.Visibility = Visibility.Visible;

                    day7.MaxNum = forecastDay.Day.MaxTemp.ToString();
                    day7.MinNum = forecastDay.Day.MinTemp.ToString();

                    day7.Day = date;

                    weatherIcon.BeginInit();
                    weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                    weatherIcon.EndInit();
                    day7.Source = weatherIcon;

                    break;
                default:
                    break;
            }
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
            windDirectionText.Text = weatherData.Current.WindDirection.ToString();

            //Pressure

            pressureLbl.Text = weatherData.Current.Pressure.ToString();

            //humidity

            humidityText.Text = weatherData.Current.Humidity.ToString();
            int humidity = int.Parse(weatherData.Current.Humidity.ToString());
            humiditySlider.Value = humidity/10;

            txtVisibility.Text = weatherData.Current.Visibility.ToString();

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

        private void close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}