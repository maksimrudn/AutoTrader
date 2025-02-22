﻿
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Helpers;
using Microsoft.Extensions.Configuration;
using System.Xml.Serialization;

namespace AutoTrader.Infrastructure.Settings
{
    public class SettingsService: ISettingsService
    {
        private readonly IConfiguration _configuration;
        private readonly string _settingsFilename = MainHelper.GetWorkFolder() + "Settings.xml";

        public SettingsService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public AutoTrader.Application.Models.Settings GetSettings()
        {
            AutoTrader.Application.Models.Settings settings = null;

            if (File.Exists(_settingsFilename))
            {
                using (FileStream fs = new FileStream(_settingsFilename, FileMode.Open))
                {
                    XmlSerializer xser = new XmlSerializer(typeof(AutoTrader.Application.Models.Settings));
                    settings = (AutoTrader.Application.Models.Settings)xser.Deserialize(fs);
                    fs.Close();
                }
            }
            else
            {
                settings = new AutoTrader.Application.Models.Settings();
            }

            return settings;
        }

        public void UpdateSettings(AutoTrader.Application.Models.Settings settings)
        {
            if (File.Exists(_settingsFilename)) File.Delete(_settingsFilename);

            using (FileStream fs = new FileStream(_settingsFilename, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(AutoTrader.Application.Models.Settings));
                xser.Serialize(fs, settings);
                fs.Close();
            }
        }
    }
}
