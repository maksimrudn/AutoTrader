using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication
{
    public static class Globals
    {
        /// <summary>
        /// Относительный путь к файлу с настройками приложения. Будет находится в папке с исполняемым файлом
        /// </summary>
        public static readonly string SettingsFile = "Settings.xml";

        public static Settings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Settings.GetSettings();
                }

                return _settings;
            }
        }

        private static Settings _settings;
    }
}
