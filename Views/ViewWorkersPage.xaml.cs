using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Sitting.Views
{
    public partial class ViewWorkersPage : ContentPage
    {
        public ViewWorkersPage()
        {
            InitializeComponent();

            // Dummy data for testing layout
            var workers = new List<Worker>
            {
                new Worker { Name = "Alice Johnson", Role = "worker" },
                new Worker { Name = "Bob Smith", Role = "worker" },
                new Worker { Name = "Charlie Davis", Role = "worker" }
            };

            WorkersList.ItemsSource = workers;
        }
    }

    // Simple model for display
    public class Worker
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}