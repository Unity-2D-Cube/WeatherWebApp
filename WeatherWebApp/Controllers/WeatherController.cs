using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherWebApp.Models;

namespace WeatherWebApp.Controllers
{
    public class WeatherController : Controller
    {
        private const string ApiKey = "3d54c044895ee1e27c19d8b0bc69f2fb";
        private const string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        private readonly WeatherIconDownloader _iconDownloader;

        public WeatherController()
        {
            _iconDownloader = new WeatherIconDownloader();
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
                return View();

            try
            {
                var weatherData = await GetWeatherDataAsync(city);

                // Iterate through the weather data and download icons
                foreach (var weather in weatherData.WeatherList)
                {
                    weather.IconPath = await _iconDownloader.DownloadIconAsync(weather.Icon);
                }

                return View(weatherData);
            }
            catch (HttpRequestException ex)
            {
                return View("Error", $"Failed to connect to the API: {ex.Message}");
            }
            catch (Exception ex)
            {
                return View("Error", $"An error occurred: {ex.Message}");
            }
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
                    if (weatherData == null || weatherData.WeatherList == null || weatherData.WeatherList.Count == 0)
                    {
                        throw new Exception("Weather data not found for the specified city.");
                    }
                    return weatherData;
                }
                else
                {
                    throw new Exception($"Failed to get weather data. Status code: {response.StatusCode}");
                }
            }
        }



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
