using Newtonsoft.Json;
using System;

namespace BotAssets
{
    public class JsonHelper
    {
        public static bool TryParse<T>(string json, out T obj)
        {
            obj = (T)Activator.CreateInstance(typeof(T));

            try { obj = JsonConvert.DeserializeObject<T>(json); }
            catch { obj = default(T); }

            return !ReferenceEquals(null, obj);
        }
    }
}
