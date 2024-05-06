using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherWebApp.Models;

namespace WeatherWebApp.Controllers
{
    public class WeatherController : Controller
    {
        private const string ApiKey = "3d54c044895ee1e27c19d8b0bc69f2fb";
        private const string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private const string IconBaseUrl = "https://openweathermap.org/img/wn/";
        private readonly WeatherIconDownloader _iconDownloader;



        public WeatherController()
        {
            _iconDownloader = new WeatherIconDownloader();
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
                return View();

            var weatherData = await GetWeatherDataAsync(city);
            // Iterate through the weather data and download icons
            foreach (var weather in weatherData.WeatherList)
            {
                weather.IconPath = await _iconDownloader.DownloadIconAsync(weather.Icon);
            }

            return View(weatherData);
        }

        private async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = $"{ApiBaseUrl}?q={city}&appid={ApiKey}&units=metric";
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                    return weatherData;
                }
                else
                {
                    throw new Exception($"Failed to get weather data. Status code: {response.StatusCode}");
                }
            }
        }

        // Dispose the WeatherIconDownloader instance
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _iconDownloader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
