using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop
{
    class AppConfiguration
    {
        //ReaderWriterLockSlim ilock = new ReaderWriterLockSlim();
        //private readonly object syncLock = new object();

        private int index = 0;

        public void UpdateAppSettings(string key, string newValue)
        {
            //Debug.WriteLine($"Update");
            //lock (syncLock)    // this is for Synchronization 
            //{
            //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    config.AppSettings.Settings[key].Value = $"{newValue}";
            //    config.Save(ConfigurationSaveMode.Minimal);

            //    Debug.WriteLine($"Update - {index++} !! /> {newValue}");

            //}

            ConstantVariable.SyncLock.WaitOne();
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = $"{newValue}";
            config.Save(ConfigurationSaveMode.Minimal);

            Debug.WriteLine($"Update - {index++} !! /> {newValue}");
            ConstantVariable.SyncLock.Release();
        }
        public string GetAppSettingsValue(string key)
        {
            //Debug.WriteLine($"GET");
            //lock (syncLock)    // this is for Synchronization 
            //{
            //    string value;
            //    value = ConfigurationManager.AppSettings[key];
            //    Debug.WriteLine($"Get - {index++} !! /> {value}");
            //    return value;
            //}

            string value;
            ConstantVariable.SyncLock.WaitOne();
            value = ConfigurationManager.AppSettings[key];
            Debug.WriteLine($"Get - {index++} !! /> {value}");
            ConstantVariable.SyncLock.Release();
            return value;

        }

        private static AppConfiguration _instance = null;

        public static AppConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppConfiguration();
                }
                return _instance;
            }
        }

        private AppConfiguration()
        {

        }
    }
}
