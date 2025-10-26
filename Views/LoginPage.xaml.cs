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
            // 🔹 Read input from the form
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            // 🔐 Validate input
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }

            // 🔹 Continue with Firebase login
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
                    var dbUrl = $"https://sittingapp-a59ae-default-rtdb.firebaseio.com/users/{uid}/role.json";
                    var roleResponse = await client.GetAsync(dbUrl);

                    string role = null;

                    if (roleResponse.IsSuccessStatusCode)
                    {
                        var roleJson = await roleResponse.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(roleJson) && roleJson != "null")
                        {
                            try
                            {
                                using var roleDoc = JsonDocument.Parse(roleJson);
                                var root = roleDoc.RootElement;

                                if (root.ValueKind == JsonValueKind.String)
                                    role = root.GetString();
                                else if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("role", out var r))
                                    role = r.GetString();
                            }
                            catch (Exception ex)
                            {
                                UidLabel.Text += $"\nRole parse error: {ex.Message}";
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"Fetched role: {role ?? "null"}");

                    // 🔀 Route based on role
                    if (role == "admin")
                    {
                        await Shell.Current.GoToAsync("/DashboardPage");
                    }
                    else if (role == "worker")
                    {
                        await Shell.Current.GoToAsync("/WorkerPage");
                    }
                    else
                    {
                        await DisplayAlert("Login Error", "User role not found or invalid. Please check Firebase.", "OK");
                    }
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