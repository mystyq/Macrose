using System.Windows;

namespace Macrose
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Create main window
            MainWindow window = new MainWindow();

            window.DataContext = new MainWindowViewModel();

            //Show main window
            window.Show();
        }
    }
}
