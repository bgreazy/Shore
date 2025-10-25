using Microsoft.Maui.Controls;
using Sitting.Services;
using System.IO;


namespace Sitting
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the main navigation shell
            MainPage = new AppShell();

            // Initialize SQLite database
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "workers.db");
            AppData.DbService = new DatabaseService(dbPath);
        }
    }

    // Global access to services
    public static class AppData
    {
        public static DatabaseService DbService;
    }
}