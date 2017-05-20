using System;
using System.Configuration;

namespace ASUVP.Core.Configuration
{
    public class ConfigManager
    {
        public static readonly string ShortDateTimeFormat = "dd.MM.yyyy";
        public static readonly string LongDateTimeFormat = "dd.MM.yyyy H:mm";

        public static TSetting AppSetting<TSetting>(string key, TSetting value = default(TSetting))
        {
            TSetting result;
            TryGetValue(key, out result);
            return result;
        }

        private static void TryGetValue<T>(string key, out T value)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            value = (T) Convert.ChangeType(appSetting, typeof (T));
        }
    }
}