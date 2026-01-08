using Microsoft.Maui.Controls;
using Sitting.ViewModels;
using System;

namespace Sitting.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            var result = await _viewModel.LoginAsync(email, password);

            if (!result.Success)
            {
                await DisplayAlert("Login Failed", result.Error, "OK");
                return;
            }

            if (result.Role == "admin")
            {
                // Admins go to DashboardPage
                await Shell.Current.GoToAsync("DashboardPage");
            }
            else if (result.Role == "worker")
            {
                // Workers go to WorkerPage
                await Shell.Current.GoToAsync("WorkerPage");
            }
            else
            {
                await DisplayAlert("Role Missing", "User has no assigned role.", "OK");
            }
        }
    }
}