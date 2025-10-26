using Microsoft.Extensions.Configuration;
using Sitting.Models;
using System.Text.Json;

namespace Sitting.Views;

public partial class ViewTasksPage : ContentPage
{
    private readonly IConfiguration _config;

    public ViewTasksPage()
    {
        InitializeComponent();
        _config = App.Current.Handler.MauiContext.Services.GetService<IConfiguration>();
        LoadTasks();
    }

    private async void LoadTasks()
    {
        try
        {
            var apiUrl = _config["Firebase:DatabaseUrl"];
            var url = $"{apiUrl}/tasks.json";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Failed to load tasks.", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var dict = JsonSerializer.Deserialize<Dictionary<string, TaskItem>>(json);

            var tasks = dict?.Values.ToList() ?? new List<TaskItem>();
            TasksCollectionView.ItemsSource = tasks;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
}