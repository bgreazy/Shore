using Microsoft.Maui.Controls;

namespace Sitting.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private async void OnViewWorkersClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///ViewWorkersPage");
        }

        private async void OnAssignTaskClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Admin Tool", "Assign Task clicked.", "OK");
        }

        private async void OnViewLogsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Admin Tool", "View Logs clicked.", "OK");
        }
        private async void OnCreateTaskClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///CreateTaskPage");
        }

        private async void OnViewTasksClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/ViewTasksPage");
        }
    }
}