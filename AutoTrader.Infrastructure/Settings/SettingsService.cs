using AutoTrader.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTrader.Application.Features.Settings
{
    public class SettingsService: IAppSettingsService
    {
        private readonly IConfiguration _configuration;
        string _settingsFilename = "Settings.xml";

        public SettingsService(IConfiguration configuration) {
            this._configuration = configuration;
        }

        public Settings GetSettings()
        {
            Settings settings = null;
            

            //проверка наличия файла
            if (File.Exists(_settingsFilename))
            {
                using (FileStream fs = new FileStream(_settingsFilename, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(Settings));
                    settings = (Settings)xser.Deserialize(fs);
                    fs.Close();
                }
            }
            else
            {
                settings = new Settings();
            }

            return settings;


            //AppSettings settings = (AppSettings)_configuration.GetSection(nameof(AppSettings));

            //return settings;
        }

        public void UpdateSettings(Settings settings)
        {
            if (File.Exists(_settingsFilename)) File.Delete(_settingsFilename);


            using (FileStream fs = new FileStream(_settingsFilename, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(Settings));
                xser.Serialize(fs, settings);
                fs.Close();
            }

            //string json = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);

            //// Write the JSON to the configuration file
            //File.WriteAllText("appsettings.json", json);
        }
    }
}
