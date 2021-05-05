using OmegaDSD.ThemeWPF.Views;
using System.Windows;

namespace OmegaDSD.ThemeWPF
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