using VertaIT.WPF.Demo.Views;
using System.Windows;

namespace VertaIT.WPF.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();
        }
    }
}