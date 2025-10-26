using Microsoft.Extensions.Configuration;
using Sitting.Models;
using System.Text;
using System.Text.Json;

namespace Sitting.Views;

public partial class CreateTaskPage : ContentPage
{
    private readonly IConfiguration _config;

    public CreateTaskPage()
    {
        InitializeComponent();
        _config = App.Current.Handler.MauiContext.Services.GetService<IConfiguration>();
    }

    private async void OnCreateTaskClicked(object sender, EventArgs e)
    {
        var title = TitleEntry.Text?.Trim();
        var description = DescriptionEditor.Text?.Trim();

        var start = StartDatePicker.Date + StartTimePicker.Time;
        var end = EndDatePicker.Date + EndTimePicker.Time;

        if (string.IsNullOrWhiteSpace(title) || end <= start)
        {
            await DisplayAlert("Error", "Please enter a valid title and time range.", "OK");
            return;
        }

        var task = new TaskItem
        {
            Title = title,
            Description = description,
            StartTime = start.ToString("s"),
            EndTime = end.ToString("s"),
            AssignedTo = null,
            Status = "available"
        };

        var json = JsonSerializer.Serialize(task);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var apiUrl = _config["Firebase:DatabaseUrl"]; // e.g., https://your-app.firebaseio.com
        var url = $"{apiUrl}/tasks.json";

        using var client = new HttpClient();
        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Success", "Task created.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Error", "Failed to create task: " + error, "OK");
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }

}
