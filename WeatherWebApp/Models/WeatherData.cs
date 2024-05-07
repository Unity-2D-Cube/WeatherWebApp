using Newtonsoft.Json;

namespace WeatherWebApp.Models
    
{
    public class WeatherData
    {
        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("weather")]
        public List<Weather> WeatherList { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // Add the CurrentTime property
        public string CurrentTime { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        // Add the IconPath property
        public string IconPath { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }
}
