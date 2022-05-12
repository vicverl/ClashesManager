using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClashesManager.Models;
using SettingsSerializer;

namespace ClashesManager.Utils
{
    public static class AppSettingsUtil
    {
        private static XMLSerializer _serializer = new (Analytics.AppName);

        public static XMLSerializer Serializer
        {
            get => _serializer;
        }

        /// <summary>
        /// Load app settings from computer
        /// </summary>
        /// <returns></returns>
        public static AppSettings LoadSettings()
        {
            AppSettings settings = Serializer.Deserialize<AppSettings>() as AppSettings;
            if (settings is null)
            {
                settings = new AppSettings();
                return settings;
            }
            return settings;
        }

        /// <summary>
        /// Save app settings to computer
        /// </summary>
        public static void SaveSettings(AppSettings appSettings)
        {
            Serializer.Serialize<AppSettings>(appSettings);
        }
    }
}
