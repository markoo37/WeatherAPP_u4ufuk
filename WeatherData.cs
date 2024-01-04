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

    public class Current
    {
        [JsonProperty("temp_c")]

        public double TempC { get; set; }

        [JsonProperty("condition")]

        public Condition Condition { get; set; }

        [JsonProperty("wind_kph")]

        public double WindSpeed { get; set; }

        [JsonProperty("humidity")]

        public double Humidity { get; set; }

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
}
