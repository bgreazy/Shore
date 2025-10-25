using System.Text.Json;

namespace Sitting.Services
{
    public static class SecretLoader
    {
        public static string GetFirebaseApiKey()
        {
            try
            {
                var json = File.ReadAllText("secrets.json");
                var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("FirebaseApiKey").GetString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SecretLoader] Failed to load API key: {ex.Message}");
                throw;
            }
        }
    }
}