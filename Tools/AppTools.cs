using Windows.Storage;

namespace MediumServerExample.Tools
{
    public class AppTools
    {
        /// <summary>
        /// Write string to local settings.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void WriteLocalSetting(Settings key, string value)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer("Manager", ApplicationDataCreateDisposition.Always);
            localcontainer.Values[key.ToString()] = value;
        }

        /// <summary>
        /// Get string to local settings.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public static string GetLocalSetting(Settings key, string defaultValue)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer("Manager", ApplicationDataCreateDisposition.Always);
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist)
            {
                return localcontainer.Values[key.ToString()].ToString();
            }
            else
            {
                WriteLocalSetting(key, defaultValue);
                return defaultValue;
            }
        }
    }
}
