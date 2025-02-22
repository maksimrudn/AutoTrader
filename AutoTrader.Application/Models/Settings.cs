﻿using AutoTrader.Application.Helpers;
using AutoTrader.Domain.Models.Strategies;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace AutoTrader.Application.Models
{
    public class Settings
    {
        #region PROPERTIES

        public int Timezone { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Username2 { get; set; }

        public string Password2 { get; set; }

        public string ConnectionType { get; set; } = "Prod";

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

        public string GetNewPassword()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(NewPassword);
            }
            catch
            {
                res = NewPassword;
            }

            return res;
        }

        public string GetOldPassword()
        {
            string res;

            try
            {
                res = Crypter.Decrypt(OldPassword);
            }
            catch
            {
                res = OldPassword;
            }

            return res;
        }

        public void SetPassword(string text)
        {
            Password = Crypter.Encrypt(text);
        }

        public void SetOldPassword(string text)
        {
            OldPassword = Crypter.Encrypt(text);
        }

        public void SetNewPassword(string text)
        {
            NewPassword = Crypter.Encrypt(text);
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

        public Settings DeepCopy()
        {
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                return (Settings)formatter.Deserialize(ms);
            }
        }
    }
}
