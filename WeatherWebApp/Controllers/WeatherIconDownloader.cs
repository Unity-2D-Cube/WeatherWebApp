namespace WeatherWebApp.Controllers
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class WeatherIconDownloader
    {
        private const string IconBaseUrl = "https://openweathermap.org/img/wn/";
        private readonly HttpClient _httpClient;

        public WeatherIconDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadIconAsync(string iconId)
        {
            try
            {
                // Get the wwwroot folder path
                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                // Combine wwwroot folder path with the folder for weather icons
                string iconFolderPath = Path.Combine(wwwRootPath, "weather-icons");

                // Ensure the folder exists, create it if it doesn't
                Directory.CreateDirectory(iconFolderPath);

                // Construct the full file path for the icon
                string iconFileName = $"{iconId}.png";
                string iconFilePath = Path.Combine(iconFolderPath, iconFileName);

                // Check if the icon file already exists
                if (!File.Exists(iconFilePath))
                {
                    // Download the icon image
                    byte[] iconData = await _httpClient.GetByteArrayAsync($"{IconBaseUrl}{iconId}@2x.png");
                    // Save the icon image to the file system
                    await File.WriteAllBytesAsync(iconFilePath, iconData);
                }

                // Return the relative path to the icon file
                return $"/weather-icons/{iconFileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading weather icon: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

    }
}
