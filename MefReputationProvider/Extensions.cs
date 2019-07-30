using System;

namespace MefReputationProvider
{
    public static class Extensions
    {
        public static T TryParse<T>(this string s)
        {
            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                t = Nullable.GetUnderlyingType(t);
            var method = t.GetMethod(
                "TryParse",
                new[] { typeof(string), t.MakeByRefType() }
                );
            T result = default(T);

            var parameters = new object[] { s, result };
            bool? parsed = null;
            if (method != null)
                parsed = (bool)method.Invoke(null, parameters);

            if (parsed.HasValue ? parsed.Value : false)
                return (T)parameters[1];
            else
                return result;
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
