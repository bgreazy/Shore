using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sitting.Services
{
    public class FirebaseAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _databaseUrl;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public FirebaseAuthService(HttpClient httpClient, string apiKey, string databaseUrl)
        {
            _httpClient = httpClient;
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _databaseUrl = databaseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(databaseUrl));
        }

        private class SignInResponse
        {
            public string LocalId { get; set; }
            public string IdToken { get; set; }
            public string RefreshToken { get; set; }
            public string Email { get; set; }
        }

        public class AuthResult
        {
            public string UserId { get; set; }
            public string Email { get; set; }
            public string IdToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public async Task<AuthResult> SignInWithEmailPasswordAsync(string email, string password)
        {
            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}";

            var payload = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Login failed: {responseBody}");

            var signIn = JsonSerializer.Deserialize<SignInResponse>(responseBody, _jsonOptions);

            return new AuthResult
            {
                UserId = signIn.LocalId,
                Email = signIn.Email,
                IdToken = signIn.IdToken,
                RefreshToken = signIn.RefreshToken
            };
        }

        public async Task<string> GetUserRoleAsync(string userId, string idToken)
        {
            var url = $"{_databaseUrl}/users/{userId}/role.json?auth={idToken}";

            var response = await _httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch role: {body}");

            if (string.IsNullOrWhiteSpace(body) || body == "null")
                return null;

            return body.Trim().Trim('"');
        }
    }
}