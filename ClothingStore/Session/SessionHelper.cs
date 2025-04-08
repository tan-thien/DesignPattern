using ClothingStore.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ClothingStore.Session
{
    public static class SessionHelper
    {
        private const string UserSessionKey = "CurrentUser";
        private const string CartSessionKey = "ShoppingCart";

        public static void SetUser(HttpContext context, UserSessionModel user)
        {
            var userData = JsonSerializer.Serialize(user);
            context.Session.SetString(UserSessionKey, userData);
            Console.WriteLine($"[DEBUG] Session Saved: {userData}");
        }

        public static UserSessionModel GetUser(HttpContext context)
        {
            var userData = context.Session.GetString(UserSessionKey);
            if (string.IsNullOrEmpty(userData))
            {
                Console.WriteLine("[ERROR] Không tìm thấy UserId trong session hoặc session bị mất!");
                return null;
            }

            Console.WriteLine($"[DEBUG] Session Retrieved: {userData}");
            return JsonSerializer.Deserialize<UserSessionModel>(userData);
        }

        public static void ClearUser(HttpContext context)
        {
            context.Session.Remove(UserSessionKey);
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value)); // ✅ Dùng System.Text.Json
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value); // ✅ Dùng System.Text.Json
        }

    }


    public class UserSessionModel
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string? Status { get; set; }
        public int IdAcc { get; set; }
        public string Role { get; set; }
    }
}
