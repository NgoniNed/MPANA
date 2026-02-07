using System.IO;
using System.Text.Json;

namespace MPANAGTK.Backend
{
    public static class Config
    {
        private static AppConfig _settings;

        public static AppConfig Settings
        {
            get
            {
                if (_settings == null)
                {
                    Load();
                }
                return _settings;
            }
        }

        public static void Load()
        {
            string fileName = "config.json";
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Critical Error: config.json not found in the application directory.");
            }

            string jsonString = File.ReadAllText(fileName);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            _settings = JsonSerializer.Deserialize<AppConfig>(jsonString, options);
        }
    }
}
