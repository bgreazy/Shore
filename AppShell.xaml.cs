using Sitting.Views;

namespace Sitting
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("DashboardPage", typeof(DashboardPage));
            Routing.RegisterRoute("WorkerPage", typeof(WorkerPage));
            Routing.RegisterRoute("ViewWorkersPage", typeof(ViewWorkersPage));
            Routing.RegisterRoute("CreateTaskPage", typeof(CreateTaskPage));
            Routing.RegisterRoute("ViewTasksPage", typeof(ViewTasksPage));
        }
    }
}