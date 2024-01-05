using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;

namespace Weather
{
    public class Location
    {
        [JsonProperty("name")]

        public string Name { get; set; }
    }

    public class Astronomy
    {
        [JsonProperty("astro")]
        public Astro Astro { get; set; }
    }

    public class Astro
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<ForecastDay> ForecastDay { get; set; }
    }

    public class ForecastDay
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        public Day Day { get; set; }

    }

    public class Day
    {
        [JsonProperty("mintemp_c")]
        public double MinTemp { get; set; }

        [JsonProperty("maxtemp_c")]
        public double MaxTemp { get; set; }

        [JsonProperty("condition")]

        public Condition Condition { get; set; }
    }

    public class Current
    {
        [JsonProperty("temp_c")]

        public double TempC { get; set; }

        [JsonProperty("condition")]

        public Condition Condition { get; set; }

        [JsonProperty("pressure_mb")]

        public double Pressure { get; set; }

        [JsonProperty("wind_kph")]

        public double WindSpeed { get; set; }

        [JsonProperty("wind_dir")]

        public string WindDirection { get; set; }

        [JsonProperty("humidity")]

        public double Humidity { get; set; }

        [JsonProperty("vis_km")]

        public double Visibility { get; set; }

        [JsonProperty("uv")]

        public double UV { get; set; }
    }

    public class Condition
    {
        [JsonProperty("text")]

        public string Text { get; set; }

        [JsonProperty("icon")]

        public string Icon { get; set; }
    }

    public class WeatherData
    {
        [JsonProperty("location")]

        public Location Location { get; set; }

        [JsonProperty("current")]

        public Current Current { get; set; }

        
    }

    public class AstronomyData
    {
        [JsonProperty("astronomy")]

        public Astronomy Astronomy { get; set; }
    }

    public class FutureData
    {
        [JsonProperty("forecast")]

        public Forecast Forecast { get; set; }
    }
}
