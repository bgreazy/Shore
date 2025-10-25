using System.Windows.Input;
using System.Text.Json;
using Sitting.Services;
using Microsoft.Maui.Controls;

namespace Sitting.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly FirebaseAuthService _authService = new();
        public string Email { get; set; }
        public string Password { get; set; }
        public string StatusMessage { get; set; }

        public ICommand LoginCommand => new Command(async () => await LoginAsync());

        private async Task LoginAsync()
        {
            try
            {
                var result = await _authService.LoginAsync(Email, Password);
                var json = JsonDocument.Parse(result);
                if (json.RootElement.TryGetProperty("idToken", out var token))
                {
                    StatusMessage = "Login successful!";
                    await Shell.Current.GoToAsync("//DashboardPage");
                }
                else
                {
                    StatusMessage = "Login failed.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }

            OnPropertyChanged(nameof(StatusMessage));
        }
    }
}