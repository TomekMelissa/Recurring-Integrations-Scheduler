using Quartz;

namespace RecurringIntegrationsScheduler.Extensions
{
    internal static class JobDataMapExtensions
    {
        public static bool GetBooleanValueOrDefault(this JobDataMap map, string key, bool defaultValue = false)
        {
            if (map == null || string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            return map.ContainsKey(key) ? map.GetBooleanValue(key) : defaultValue;
        }

        public static string GetStringOrDefault(this JobDataMap map, string key, string defaultValue = null)
        {
            if (map == null || string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            return map.ContainsKey(key) ? map.GetString(key) : defaultValue;
        }

        public static int GetIntOrDefault(this JobDataMap map, string key, int defaultValue = 0)
        {
            if (map == null || string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }

            return map.ContainsKey(key) ? map.GetInt(key) : defaultValue;
        }
    }
}
