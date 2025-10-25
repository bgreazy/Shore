using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Sitting
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = "admin@test.com"; // Replace with your test email
            var password = "yourTestPassword"; // Replace with your test password
            var apiKey = "YOUR_FIREBASE_WEB_API_KEY"; // Replace with your actual Firebase Web API key

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

            // Show raw response for debugging
            UidLabel.Text = "Raw response: " + result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    using var doc = JsonDocument.Parse(result);
                    string uid = doc.RootElement.GetProperty("localId").GetString();
                    UidLabel.Text = "UID: " + uid;
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