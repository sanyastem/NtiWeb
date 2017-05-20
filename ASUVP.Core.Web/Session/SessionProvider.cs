using System.Web;

namespace ASUVP.Core.Web.Session
{
    public static class SessionProvider
    {
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static T Get<T>(string key)
        {
            var value = HttpContext.Current.Session[key];
            return value != null ? (T) value : default(T);
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}