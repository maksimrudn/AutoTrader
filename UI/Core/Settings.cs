using AutoTraderUI.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoTraderUI.Core
{
    public class Settings
    {
        #region FILE READ/WRITE
        /// <summary>
        /// Функция получения данных настроек приложения из файла
        /// </summary>
        /// <returns></returns>
        public static Settings GetSettings()
        {
            Settings settings = null;
            string filename = Globals.SettingsFile;

            //проверка наличия файла
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
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
        }



        /// <summary>
        /// Процедура сохранения настроек в файл
        /// </summary>
        public void Save()
        {
            string filename = Globals.SettingsFile;

            if (File.Exists(filename)) File.Delete(filename);


            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer xser = new XmlSerializer(typeof(Settings));
                xser.Serialize(fs, this);
                fs.Close();
            }
        }

        #endregion

        #region PROPERTIES

        public int Timezone { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Username2 { get; set; }

        public string Password2 { get; set; }

        public string ConnectionType { get; set; }

        public string Seccode { get; set; }

        public int TP { get; set; }

        public int SL { get; set; }

        public int Price { get; set; }

        public int Volume { get; set; }

        public bool ByMarket { get; set; }

        // 1 - condition
        // 2 - stop
        public int ComboOrderType { get; set; } = 1;
        public DateTime MultidirectExecuteTime { get; set; } = DateTime.Now;
        public bool Shutdown { get; set; }


        #endregion

        public void SetUsername(string text)
        {
            Username = Crypter.Encrypt(text);
        }
        public string GetUsername()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Username);
            }
            catch
            {
                res = Username;
            }

            return res;
        }

        public string GetPassword()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Password);
            }
            catch
            {
                res = Password;
            }

            return res;
        }

        public void SetPassword(string text)
        {
            Password = Crypter.Encrypt(text);
        }

        public void SetUsername2(string text)
        {
            Username2 = Crypter.Encrypt(text);
        }
        public string GetUsername2()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Username2);
            }
            catch
            {
                res = Username2;
            }

            return res;
        }

        public string GetPassword2()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(Password2);
            }
            catch
            {
                res = Password2;
            }

            return res;
        }

        public void SetPassword2(string text)
        {
            Password2 = Crypter.Encrypt(text);
        }

        public StrategiesCollectionSettings StrategiesCollection { get; set; } = new StrategiesCollectionSettings();
    }
}
