using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Sitting.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly IConfiguration _config;

        public LoginPage()
        {
            InitializeComponent();
            _config = App.Current.Handler.MauiContext.Services.GetService<IConfiguration>();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = _config["Firebase:TestEmail"];
            var password = _config["Firebase:TestPassword"];
            var apiKey = _config["Firebase:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                UidLabel.Text = "API key missing from configuration.";
                return;
            }

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";

            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    using var doc = JsonDocument.Parse(result);
                    string uid = doc.RootElement.GetProperty("localId").GetString();
                    UidLabel.Text = "UID: " + uid;

                    // 🔥 Fetch role from Firebase Realtime Database
                    var dbUrl = $"https://sittingapp-a59ae-default-rtdb.firebaseio.com/users/{uid}/role.json"; // Replace with your actual Firebase project ID
                    var roleResponse = await client.GetAsync(dbUrl);
                    var roleJson = await roleResponse.Content.ReadAsStringAsync();

                    using var roleDoc = JsonDocument.Parse(roleJson);
                    var roleElement = roleDoc.RootElement;

                    string role = roleElement.ValueKind switch
                    {
                        JsonValueKind.String => roleElement.GetString(),
                        JsonValueKind.Object when roleElement.TryGetProperty("role", out var r) => r.GetString(),
                        _ => null
                    };

                    // 🔀 Route based on role
                    if (role == "admin")
                    {
                        await Shell.Current.GoToAsync("//DashboardPage");
                    }
                    else if (role == "worker")
                    {
                        await Shell.Current.GoToAsync("//WorkerPage");
                    }
                    else
                    {
                        UidLabel.Text += $"\nUnknown role: {role}";
                    }

                    System.Diagnostics.Debug.WriteLine("UID: " + uid);
                }
                catch (Exception ex)
                {
                    UidLabel.Text = "UID parse error: " + ex.Message;
                }
            }
            else
            {
                UidLabel.Text = "Login failed: " + result;
            }
        }
    }
}