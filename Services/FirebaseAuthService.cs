using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Sitting.Services
{
    public class FirebaseAuthService
    {
        private static readonly string ApiKey = SecretLoader.GetFirebaseApiKey();
        

        public async Task<string> LoginAsync(string email, string password)
        {
            var payload = new
            {
                email,
                password,
                returnSecureToken = true
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await new HttpClient().PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}",
                content
            );
            return await response.Content.ReadAsStringAsync();
        }
    }
}