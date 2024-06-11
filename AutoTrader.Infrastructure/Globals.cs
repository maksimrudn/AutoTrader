using AutoTrader.Application.Features.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure
{
    public static class Globals
    {
        /// <summary>
        /// Относительный путь к файлу с настройками приложения. Будет находится в папке с исполняемым файлом
        /// </summary>
        public static readonly string SettingsFile = "Settings.xml";

        public static AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = AppSettings.GetSettings(Globals.SettingsFile);
                }

                return _settings;
            }
        }

        private static AppSettings _settings;

        public const string ProductName = "Autotrader";

        public const string NotificationEmail = "admin@viclouds.ru";

        public const string NotificationEmailPassword = "Qwer1234";

        public const string SMTPServer = "192.168.0.4";

        public const int SMTPPort = 25;
    }
}
