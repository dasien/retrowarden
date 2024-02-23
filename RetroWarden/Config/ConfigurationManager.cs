
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Retrowarden.Config
{
    public sealed class ConfigurationManager
    {
        // This constructor prevents the compiler from creating a default one which can be called by clients.
        ConfigurationManager()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.AddJsonFile("appsettings.json");
            
            // Read the configuration from file.
            IConfigurationRoot root = builder.Build();
            
            // Get the config object.
            _config = root.Get<RetrowardenConfig>();

        }
        
        private static readonly object _lockObject = new object();
        private static ConfigurationManager _instance = null;
        private static RetrowardenConfig _config;
        
        public static ConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            // Return new instance
                            _instance = new ConfigurationManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public void WriteConfig(RetrowardenConfig config)
        {
            // Create new options bag for serialization.
            JsonSerializerOptions jsonWriteOptions = new JsonSerializerOptions();
            
            // Set write options.
            jsonWriteOptions.WriteIndented = true;
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());
            
            // Get new config json.
            string newJson = JsonSerializer.Serialize(config, jsonWriteOptions);
            
            // Set file location.
            string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            
            // Write the new appsettings file.
            File.WriteAllText(appSettingsPath, newJson);
        }
        
        public RetrowardenConfig GetConfig()
        {
            return _config;
        }
    }
}

