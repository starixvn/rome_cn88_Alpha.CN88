namespace CN88.Auth.Services
{
    using System.Web;
    using System.Web.SessionState;

    public static class SessionHelper
    {
        public static T Get<T>(string key)
        {
            var session = HttpSession;
            var value = session[BuildKey(session.SessionID, key)];

            return value == null ? default(T) : (T)value;
        }

        public static void Add(string key, object item)
        {
            if (item != null)
            {
                var session = HttpSession;
                session[BuildKey(session.SessionID, key)] = item;
            }
        }

        public static void Remove(string key)
        {
            var session = HttpSession;
            session.Remove(BuildKey(session.SessionID, key));
        }

        private static string BuildKey(string sessionId, string key)
            => $"{sessionId}_{key}";

        private static HttpSessionState HttpSession
            => HttpContext.Current.Session;
    }
}
