using System.Text.Json;

namespace ProjectNet.Controllers
{
    internal static class SessionExtensionsHelpers
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}