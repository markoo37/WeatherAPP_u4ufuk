using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
                MessageBox.Show("Kérem írjon be egy városnevet!");
                return;
            }

            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}";
            string apiUrlAstronomy = $"http://api.weatherapi.com/v1/astronomy.json?key={apiKey}&q={cityName}";
            string apiForecastUrl = $"http://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={cityName}&days=8&aqi=no&alerts=no";

            int errorCount = 0;


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

                MessageBox.Show("Hiba lépett fel az időjárás lekérdezése közben: " + ex.Message);
                errorCount++;
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
                if (errorCount == 0)
                {
                    MessageBox.Show("Hiba lépett fel az időjárás lekérdezése közben: " + ex.Message);
                }
                
                errorCount++;
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
                                futureDatas.Add(futureData);
                                //DisplayFutureData(futureData, i, currentDay.AddDays(13 + i).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {

                MessageBox.Show("Hiba lépett fel az időjárás lekérdezése közben: " + ex.Message);
            }


            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiForecastUrl);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            ForecastData forecastData = JsonConvert.DeserializeObject<ForecastData>(jsonResponse);
                            DisplayForecastData(forecastData);
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                
                if (errorCount == 0)
                {
                    MessageBox.Show("Hiba lépett fel az időjárás lekérdezése közben: " + ex.Message);
                }

               
                errorCount++;
            }
            if (errorCount == 0)
            {
                cardStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                cardStackPanel.Visibility = Visibility.Hidden;
            }

            pickerPanel.Visibility = Visibility.Visible;
            infoStackPanel.Visibility = Visibility.Visible;
        }

        private void DisplayForecastData(ForecastData futureData)
        {
            BitmapImage weatherIcon = new BitmapImage();
            for (int i = 1; i < futureData.Forecast.ForecastDay.Count; i++)
            {
                ForecastDay forecastDay = futureData.Forecast.ForecastDay[i];
                switch (i)
                {
                    case 1:
                        day1.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day1.MinNum = forecastDay.Day.MinTemp.ToString();

                        day1.Day = "Holnap";

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day1.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 2:
                        day2.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day2.MinNum = forecastDay.Day.MinTemp.ToString();

                        day2.Day = "Holnapután";

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day2.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 3:
                        day3.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day3.MinNum = forecastDay.Day.MinTemp.ToString();

                        day3.Day = FirstLetterToUpper(DateTime.Parse(forecastDay.Date).ToString("dddd"));

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day3.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 4:
                        day4.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day4.MinNum = forecastDay.Day.MinTemp.ToString();

                        day4.Day = FirstLetterToUpper(DateTime.Parse(forecastDay.Date).ToString("dddd"));

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day4.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 5:
                        day5.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day5.MinNum = forecastDay.Day.MinTemp.ToString();

                        day5.Day = FirstLetterToUpper(DateTime.Parse(forecastDay.Date).ToString("dddd"));

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day5.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 6:
                        day6.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day6.MinNum = forecastDay.Day.MinTemp.ToString();

                        day6.Day = FirstLetterToUpper(DateTime.Parse(forecastDay.Date).ToString("dddd"));

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day6.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 7:
                        day7.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day7.MinNum = forecastDay.Day.MinTemp.ToString();

                        
                        day7.Day = FirstLetterToUpper(DateTime.Parse(forecastDay.Date).ToString("dddd"));

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day7.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    default:
                        break;
                }
            }
            
        }

        private string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        private void DisplayAstronomyData(AstronomyData astronomyData)
        {
            //sunrise/sunset

            lblSunrise.Text = astronomyData.Astronomy.Astro.Sunrise;
            lblSunset.Text = astronomyData.Astronomy.Astro.Sunset;
        }

        private List<FutureData> futureDatas = new List<FutureData>();


        private void DisplayFutureData()
        {
            for (int i = 0; i < futureDatas.Count; i++)
            {
                ForecastDay forecastDay = futureDatas[i].Forecast.ForecastDay.First();
                BitmapImage weatherIcon = new BitmapImage();

                switch (i + 1)
                {
                    case 1:
                        day1.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day1.MinNum = forecastDay.Day.MinTemp.ToString();

                        day1.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day1.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 2:
                        day2.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day2.MinNum = forecastDay.Day.MinTemp.ToString();

                        day2.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day2.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 3:
                        day3.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day3.MinNum = forecastDay.Day.MinTemp.ToString();

                        day3.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day3.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 4:
                        day4.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day4.MinNum = forecastDay.Day.MinTemp.ToString();

                        day4.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day4.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 5:
                        day5.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day5.MinNum = forecastDay.Day.MinTemp.ToString();

                        day5.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day5.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 6:
                        day6.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day6.MinNum = forecastDay.Day.MinTemp.ToString();

                        day6.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day6.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    case 7:
                        day7.MaxNum = forecastDay.Day.MaxTemp.ToString();
                        day7.MinNum = forecastDay.Day.MinTemp.ToString();

                        day7.Day = DateTime.Parse(forecastDay.Date).ToString("m");

                        weatherIcon.BeginInit();
                        weatherIcon.UriSource = new Uri("http:" + forecastDay.Day.Condition.Icon);
                        weatherIcon.EndInit();
                        day7.Source = weatherIcon;
                        weatherIcon = new BitmapImage();
                        break;
                    default:
                        break;
                }
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
            pressureSlider.Value = weatherData.Current.Pressure - 300;
            pressureSlider.IsEnabled = false;

            //humidity

            humidityText.Text = weatherData.Current.Humidity.ToString();
            int humidity = int.Parse(weatherData.Current.Humidity.ToString());
            humiditySlider.Value = humidity/10;
            humiditySlider.IsEnabled = false;

            txtVisibility.Text = weatherData.Current.Visibility.ToString();

            //uv

            uvSlider.Value = weatherData.Current.UV;
            uvSlider.IsEnabled = false;
            uvText.Text = $"A napi átlag UV index {weatherData.Current.UV}";

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

        private async void PresentThisWeek(object sender, MouseButtonEventArgs e)
        {



            string apiForecastUrl = $"http://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={cityName}&days=8&aqi=no&alerts=no";
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(apiForecastUrl);
                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            ForecastData forecastData = JsonConvert.DeserializeObject<ForecastData>(jsonResponse);
                            DisplayForecastData(forecastData);
                        }
                    }
                }

            }
            catch (WebException ex)
            {

                MessageBox.Show("Hiba lépett fel az időjárás lekérdezése közben: " + ex.Message);
            }

            thisWeekLabel.Foreground = Brushes.Blue;
            futureLabel.Foreground = Brushes.Black;
        }

        private void PresentFuture(object sender, MouseButtonEventArgs e)
        {
            DisplayFutureData();

            thisWeekLabel.Foreground = Brushes.Black;
            futureLabel.Foreground = Brushes.Blue;
        }

        private void minimizeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}